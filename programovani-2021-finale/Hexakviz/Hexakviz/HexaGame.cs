using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Hexakviz
{
    class HexaGame
    {
        public HexaGame(Grid container, int sideLength)
        {
            this.container = container;
            this.sideLength = sideLength;

            startGame();
        }

        private string gameId;

        private int sideLength;
        private Grid container;
        private Polygon[,] hexagons;
        private Label[,] labels;
        private int[] hexagonStates;
        private bool clickStarted = false;

        private Grid questionGrid;

        private void startGame()
        {
            if (startGameOnServerAsync().GetAwaiter().GetResult())
            {
                generateHexagons();

                resizeHexagons();
            }
            else
            {
                MessageBox.Show("Nepodařilo se založit hru");
            }
        }

        private async Task<bool> startGameOnServerAsync()
        {
            using (var client = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Post, "http://hexa-kviz.proed.cz/api/game/" + sideLength))
            {
                using (HttpResponseMessage response = await client
                    .SendAsync(request, HttpCompletionOption.ResponseHeadersRead)
                    .ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();

                    string jsonGame = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    dynamic game = JsonConvert.DeserializeObject(jsonGame);

                    this.gameId = game.game;
                    return true;
                }
            }
        }

        private void generateHexagons()
        {
            hexagons = new Polygon[sideLength, sideLength];
            labels = new Label[sideLength, sideLength];

            int number = 0;

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    number++;
                    Polygon hexagon = new Polygon
                    {
                        //StreamGeometry.Parse("M5,0 L0.66,2.5 0.66,7.5 5,10 9.33,7.5 9.33,2.5 Z"),
                        Points = new PointCollection
                        {
                            new Point(5, 0),
                            new Point(0.66, 2.5),
                            new Point(0.66, 7.5),
                            new Point(5, 10),
                            new Point(9.33, 7.5),
                            new Point(9.33, 2.5),
                        },
                        Stretch = Stretch.Uniform,
                        Stroke = Brushes.Black,
                        StrokeThickness = 3,
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        Tag = number - 1,
                    };
                    Label label = new Label
                    {
                        VerticalAlignment = VerticalAlignment.Top,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalContentAlignment = VerticalAlignment.Center,
                        HorizontalContentAlignment = HorizontalAlignment.Center,
                        Content = number,
                        Tag = number - 1,
                    };

                    label.MouseEnter += HexagonLabel_MouseEnter;
                    label.MouseLeave += HexagonLabel_MouseLeave;
                    label.MouseDown += HexagonLabel_MouseDown;
                    label.MouseUp += HexagonLabel_MouseUp;

                    container.Children.Add(hexagon);
                    container.Children.Add(label);

                    hexagons[i, j] = hexagon;
                    labels[i, j] = label;
                }
            }

            hexagonStates = new int[number + 1];
        }

        public void resizeHexagons()
        {
            if (hexagons == null)
            {
                return;
            }
            double shorterContainerLenght = Math.Min(container.ActualWidth, container.ActualHeight);

            double hexagonLength = shorterContainerLenght / sideLength * 0.9;
            double marginLength = shorterContainerLenght / sideLength * 0.1;

            for (int i = 0; i < sideLength; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    Polygon hexagon = hexagons[i, j];
                    Label label = labels[i, j];

                    hexagon.Width = hexagonLength;
                    hexagon.Height = hexagonLength;

                    hexagon.StrokeThickness = hexagonLength / 20;

                    label.Width = hexagonLength;
                    label.Height = hexagonLength;

                    label.FontSize = hexagonLength / 2;

                    Thickness margin = getFieldsMargin(i, j, hexagonLength, marginLength);


                    hexagon.Margin = margin;
                    label.Margin = margin;
                }
            }
        }

        private Thickness getFieldsMargin(int i, int j, double hexagonLength, double marginLength)
        {
            Thickness margin;

            double marginTop = i * (hexagonLength + marginLength / 2);
            double distanceFromMiddle = i / 2d - j;
            if (i % 2 == 0)
            {
                if (distanceFromMiddle > 0)
                {
                    margin = new Thickness(-distanceFromMiddle * (marginLength + 2 * hexagonLength), marginTop, 0, 0);
                }
                else if (distanceFromMiddle < 0)
                {
                    margin = new Thickness(0, marginTop, distanceFromMiddle * (marginLength + 2 * hexagonLength), 0);
                }
                else
                {
                    margin = new Thickness(0, marginTop, 0, 0);
                }
            }
            else
            {
                if (distanceFromMiddle < 0)
                {
                    margin = new Thickness(-distanceFromMiddle * 2 * hexagonLength + -(distanceFromMiddle - 0.25) * marginLength, marginTop, 0, 0);
                }
                else //(distanceFromMiddle < 0)
                {
                    margin = new Thickness(0, marginTop, distanceFromMiddle * 2 * hexagonLength + (distanceFromMiddle + 0.25) * marginLength, 0);
                }
            }

            return margin;
        }

        private Polygon findHexagonByTag(int tag)
        {
            foreach (Polygon item in hexagons)
            {
                if (item != null)
                {
                    if ((int)item.Tag == tag)
                    {
                        return item;
                    }
                }
            }
            return null;
        }

        private void HexagonLabel_MouseEnter(object sender, MouseEventArgs e)
        {
            if (sender is Label label)
            {
                Polygon hexagon = findHexagonByTag((int)label.Tag);

                ScaleTransform transform = new ScaleTransform(1.2, 1.2, hexagon.Width / 2, hexagon.Width / 2);

                label.RenderTransform = transform;
                hexagon.RenderTransform = transform;
            }
        }

        private void HexagonLabel_MouseLeave(object sender, MouseEventArgs e)
        {
            clickStarted = false;
            if (sender is Label label)
            {
                Polygon hexagon = findHexagonByTag((int)label.Tag);

                label.RenderTransform = null;
                hexagon.RenderTransform = null;
            }
        }

        private void HexagonLabel_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label)
            {
                clickStarted = true;
            }
        }

        private void HexagonLabel_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is Label label)
            {
                if (clickStarted)
                {
                    Polygon hexagon = findHexagonByTag((int)label.Tag);

                    if (hexagonStates[(int)hexagon.Tag] == 0)
                    {
                        Question question = new Question(this.gameId, (int)hexagon.Tag + 1);
                        showQuestion(question);

                        Random rnd = new Random();
                        
                        if (question.tryAnswer(rnd.Next(4)))
                        {
                            hexagon.Fill = Brushes.GreenYellow;
                            hexagonStates[(int)hexagon.Tag] = 1;
                        }
                        else
                        {
                            hexagon.Fill = Brushes.Gray;
                            hexagonStates[(int)hexagon.Tag] = 2;
                        }
                    }
                }
            }
        }

        private void showQuestion(Question question)
        {
            questionGrid = new Grid();

            questionGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            questionGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(8, GridUnitType.Star)
            });
            questionGrid.ColumnDefinitions.Add(new ColumnDefinition
            {
                Width = new GridLength(1, GridUnitType.Star)
            });

            questionGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(2, GridUnitType.Star)
            });
            questionGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(5, GridUnitType.Star)
            });
            questionGrid.RowDefinitions.Add(new RowDefinition
            {
                Height = new GridLength(1, GridUnitType.Star)
            });

            container.Children.Add(questionGrid);

            Grid gridQuestion = new Grid();
            gridQuestion.SetValue(Grid.ColumnProperty, 1);
            gridQuestion.SetValue(Grid.RowProperty, 1);

            Polygon questionField = new Polygon
            {
                Points = new PointCollection
                        {
                            new Point(5, 0),
                            new Point(0.66, 2.5),
                            new Point(0.66, 7.5),
                            new Point(5, 10),
                            new Point(15, 10),
                            new Point(19.33, 7.5),
                            new Point(19.33, 2.5),
                            new Point(15, 0),
                        },
                Stretch = Stretch.Uniform,
                Stroke = Brushes.Black,
                StrokeThickness = 20,
            };
            gridQuestion.Children.Add(questionField);
        }

        private void removeQuestion()
        {
            container.Children.Remove(questionGrid);
        }
    }
}
