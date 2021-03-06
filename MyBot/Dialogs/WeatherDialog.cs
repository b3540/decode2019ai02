﻿using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Choices;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyBot.Dialogs
{
    public class WeatherDialog : ComponentDialog
    {
        private static IList<Choice> choices = ChoiceFactory.ToChoices(new List<string>() { "今日", "明日", "明後日" });
        public WeatherDialog() : base(nameof(WeatherDialog))
        {
            // ウォーターフォールのステップを定義。処理順にメソッドを追加。
            var waterfallSteps = new WaterfallStep[]
            {
                AskDateAsync,
                ShowWeatherAsync,
            };

            // ウォーターフォールダイアログと各種プロンプトを追加
            AddDialog(new WaterfallDialog("weather", waterfallSteps));
            AddDialog(new ChoicePrompt("choice"));
        }

        private static async Task<DialogTurnResult> AskDateAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            if (stepContext.Options != null && !string.IsNullOrEmpty(stepContext.Options.ToString()))
            {
                return await stepContext.NextAsync(new FoundChoice() { Value = stepContext.Options.ToString() });
            }
            else
            {
                // Choice プロンプトでメニューを表示
                return await stepContext.PromptAsync(
                    "choice",
                    new PromptOptions
                    {
                        Prompt = MessageFactory.Text("いつの天気を知りたいですか？"),
                        Choices = choices,
                    },
                    cancellationToken);
            }
        }

        private static async Task<DialogTurnResult> ShowWeatherAsync(WaterfallStepContext stepContext, CancellationToken cancellationToken)
        {
            var choice = (FoundChoice)stepContext.Result;
            await stepContext.Context.SendActivityAsync($"{choice.Value}の天気は晴れです");
            return await stepContext.EndDialogAsync(true, cancellationToken);
        }
    }
}
