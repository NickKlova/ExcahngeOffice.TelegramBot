using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Cache.Clients.Interfaces;
using ExchangeOffice.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Newtonsoft.Json;
using System.Reflection;
using Telegram.Bot.Types;

public class StepperMiddleware {
	private readonly RequestDelegate _next;
	private readonly ICacheClient _cache;

	public StepperMiddleware(RequestDelegate next, ICacheClient cacheClient) {
		_next = next;
		_cache = cacheClient;
	}

	public async Task InvokeAsync(HttpContext context, IServiceProvider provider) {
		context.Request.EnableBuffering();
		var update = await GetUpdateFromRequest(context.Request.Body);
        if (update == null) {
			return;
		}
   
        string? userStepper = GetUserStepper(update);
		if (string.IsNullOrEmpty(userStepper)) {
			return;
		}

		var stepInfoJson = _cache.Get(userStepper);
		if (!string.IsNullOrEmpty(stepInfoJson)) {
			var stepInfo = JsonConvert.DeserializeObject<StepperInfo>(stepInfoJson);
			if (stepInfo == null) {
				return;
			}

			await ExecuteTextHandlerNextStep(update, stepInfo);
		}
		
		await _next(context);
	}

	private void DeleteFinishedStep(string key) {
		_cache.Delete(key);
	}

	private void IncrementStep(string key, StepperInfo info) {
		if (++info.CurrentStep >= info.StepsCount) {
			DeleteFinishedStep(key);
		}
		var json = JsonConvert.SerializeObject(info);
		_cache.Set(key, json);
	}

	private async Task ExecuteTextHandlerNextStep(Update request, StepperInfo info) {
		var stepperKey = GetUserStepper(request);
		if (string.IsNullOrEmpty(stepperKey))
			return;

		var types = GetTypesWithTextHandlerAttribute(info.Name);

		foreach (var type in types) {
			MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
			foreach (var method in methods) {
				var attributes = method.GetCustomAttributes(typeof(TextStepperAttribute), false);
				foreach (var attribute in attributes) {
					var typedAttribute = (TextStepperAttribute)attribute;
					if (typedAttribute.Name == info.Name && typedAttribute.Step == info.CurrentStep + 1) {
						var instance = Activator.CreateInstance(type);
						await Task.FromResult(method.Invoke(instance, new[] { request }));
						IncrementStep(stepperKey, info);
					}
				}
			}
		}
	}

	private IEnumerable<Type> GetTypesWithTextHandlerAttribute(string stepperName) {
		Assembly assembly = Assembly.GetExecutingAssembly();
		var types = assembly.GetTypes();

		return types.Where(t =>
			t.GetCustomAttributes(typeof(TextMessageHandlerAttribute), false)
				.Any(attr => ((TextMessageHandlerAttribute)attr).Text == stepperName));
	}

	private string? GetUserStepper(Update request) {
		return request?.Message?.Chat?.Id.ToString();
	}

	private async Task<Update?> GetUpdateFromRequest(Stream requestBody) {
		var body = await GetRequestBody(requestBody);
		return JsonConvert.DeserializeObject<Update>(body);
	}

	private async Task<string> GetRequestBody(Stream body) {
		var data = await new StreamReader(body, leaveOpen: true).ReadToEndAsync();
		if (body.CanSeek)
			body.Position = 0;
		return data;
	}
}
