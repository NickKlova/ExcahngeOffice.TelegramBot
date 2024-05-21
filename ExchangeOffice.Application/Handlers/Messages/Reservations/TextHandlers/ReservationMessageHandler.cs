using ExchangeOffice.Application.Attributes;
using ExchangeOffice.Application.Constants;
using ExchangeOffice.Application.Extensions.Providers.Interfaces;
using ExchangeOffice.Application.Handlers.Messages.Abstractions;
using ExchangeOffice.Application.Handlers.Messages.Interfaces;
using ExchangeOffice.Application.Managers.Interfaces;
using ExchangeOffice.Application.Views.ReplyMarkups;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace ExchangeOffice.Application.Handlers.Messages.Reservations.TextHandlers
{
    [TextMessageHandler(MenuTitles.Reservations)]
    public class ReservationMessageHandler : BaseMessageHandler, ITextMessageHandler
    {
        private readonly ITelegramBotClient _bot;
        private readonly IContactManager _contactManager;
        public ReservationMessageHandler(IManagerProvider managerProvider) : base(managerProvider)
        {
            _bot = managerProvider.GetTelegramBotClient();
            _contactManager = managerProvider.GetContactManager();
        }

        public async Task ExecuteAsync(Update request)
        {
            var chatId = request?.Message?.Chat.Id;
            if (chatId == null)
            {
                return;
            }

            await _bot.SendTextMessageAsync(chatId, "Choose option:", replyMarkup: ReservationMenu.Buttons);
        }
    }
}
