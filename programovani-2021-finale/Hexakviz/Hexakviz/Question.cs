using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace Hexakviz
{
    internal class Question
    {
        public Question(string gameId, int hexId)
        {
            this.gameId = gameId;
            this.hexId = hexId;

            loadQuestionAsync().GetAwaiter().GetResult();
        }

        private string gameId;
        private int hexId;

        private string questionId;
        private string question;
        private string[] answers;

        private async Task<bool> loadQuestionAsync()
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, "http://hexa-kviz.proed.cz/api/question/" + hexId))
            {
                request.Headers.Add("X-Game", gameId);

                using (HttpResponseMessage response = await client
                    .SendAsync(request)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    string jsonQuestion = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    dynamic question = JsonConvert.DeserializeObject(jsonQuestion);

                    this.questionId = question.id;
                    this.question = question.question;
                    this.answers = question.answers.ToObject<string[]>();

                    return true;
                }
            }
        }

        public bool tryAnswer(int answerId)
        {
            return tryAnswerAsync(answerId).GetAwaiter().GetResult();
        }

        private async Task<bool> tryAnswerAsync(int answerId)
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, "http://hexa-kviz.proed.cz/api/answer/" + hexId))
            {
                request.Headers.Add("X-Game", gameId);
                var values = new Dictionary<string, string>
                {
                    { "id", questionId },
                    { "answer", answerId.ToString() }
                };

                HttpContent content = new StringContent(JsonConvert.SerializeObject(values));

                request.Content = content;

                using (HttpResponseMessage response = await client
                    .SendAsync(request)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    string jsonAnswer = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    dynamic answer = JsonConvert.DeserializeObject(jsonAnswer);

                    if (answer.result == 1)
                    {
                        return true;
                    }
                    else
                    {
                        int correctAnswer = answer.correct_answer;
                        string reason = answer.reason;
                        return false;
                    }
                }
            }
        }
    }
}