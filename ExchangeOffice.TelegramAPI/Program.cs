using ExchangeOffice.Core.Extensions;
using Telegram.Bot;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddNewtonsoftJson();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationLayer();
var client = new TelegramBotClient("6700447814:AAG-ynj_oHoZ9mEeW8kORRCSXDl0Aewf2i0");
client.SetWebhookAsync("https://c7e5-176-39-34-13.ngrok-free.app/api/telegram/update/", dropPendingUpdates: true).Wait();
builder.Services.AddSingleton<ITelegramBotClient>(options => {
	
	return client;
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
	app.UseSwagger();
	app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.UseApplicationLayer();

app.Run();
