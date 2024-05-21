using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.ReplyMarkups;

namespace ExchangeOffice.Application.Views.InlineMarkups {
	public static class RatesMenu {
		public static readonly InlineKeyboardMarkup Buttons = new (new[]
        {
            // first row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "Accepted currencies", callbackData: "str"),
                InlineKeyboardButton.WithCallbackData(text: "usd", callbackData: "12"),
            },
            // second row
            new []
            {
                InlineKeyboardButton.WithCallbackData(text: "btc", callbackData: "21"),
                InlineKeyboardButton.WithCallbackData(text: "eur", callbackData: "22"),
            },
        });
	}
}
