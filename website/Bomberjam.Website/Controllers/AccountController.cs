using System;
using System.Linq;
using System.Threading.Tasks;
using Bomberjam.Website.Database;
using Bomberjam.Website.Models;
using Bomberjam.Website.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace Bomberjam.Website.Controllers
{
    [Authorize]
    [Route("~/account")]
    public class AccountController : BaseWebController<AccountController>
    {
        public AccountController(IRepository repository, IBotStorage botStorage, ILogger<AccountController> logger)
            : base(repository, logger)
        {
            this.BotStorage = botStorage;
        }

        private IBotStorage BotStorage { get; }

        public async Task<IActionResult> Index()
        {
            var user = await this.GetAuthenticatedUser();
            var bots = await this.Repository.GetBots(user.Id);
            return this.View("Index", new AccountReadViewModel(user, bots));
        }

        [HttpGet("edit")]
        public async Task<IActionResult> Edit()
        {
            var user = await this.GetAuthenticatedUser();
            var viewModel = new AccountEditViewModel
            {
                UserName = user.UserName
            };

            return this.View("Edit", viewModel);
        }

        [HttpPost("edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(AccountEditViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Edit", viewModel);

            var user = await this.GetAuthenticatedUser();
            user.UserName = viewModel.UserName;
            await this.Repository.UpdateUser(user);

            return this.RedirectToAction("Index", "Account");
        }

        [HttpGet("submit")]
        public IActionResult Submit()
        {
            return this.View("Submit", new AccountSubmitViewModel());
        }

        private static readonly TimeSpan BotSubmitDelay = TimeSpan.FromMinutes(1);

        [HttpPost("submit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Submit(AccountSubmitViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
                return this.View("Submit", viewModel);

            var user = await this.GetAuthenticatedUser();
            var bots = await this.Repository.GetBots(user.Id);

            if (bots.OrderByDescending(b => b.Updated).FirstOrDefault() is { } mostRecentBot && DateTime.UtcNow - mostRecentBot.Updated < BotSubmitDelay)
            {
                this.ModelState.AddModelError<AccountSubmitViewModel>(vm => vm.BotFile, "You need to wait more before submitting a new bot");
                return this.View("Submit", viewModel);
            }

            await this.BotStorage.UploadBotSourceCode(user.Id, viewModel.BotFile.OpenReadStream());
            var newBotId = await this.Repository.AddBot(user.Id);
            await this.Repository.AddCompilationTask(newBotId);

            return this.RedirectToAction("Index", "Account");
        }
    }
}
