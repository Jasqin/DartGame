using Microsoft.Maui.Controls;
using System;
using System.Collections.Generic;
using System.Linq;


namespace DartGame
{
    public partial class MainPage : ContentPage
    {
        private int player1Score = 501;
        private int player2Score = 501;
        private int player3Score = 501;
        private int numberOfPlayers = 2;
        private bool isPlayer1Turn = true;
        private bool isPlayer2Turn = false;
        private bool isPlayer3Turn = false;
        private List<int> currentRoundPoints = new List<int>();
        private List<bool> lastThrowModified = new List<bool>();
        private List<bool> lastThrowTripled = new List<bool>();
        private List<bool> lastThrowDoubled = new List<bool>();
        private int playerToGetOut = 3;
        private int doubleCount = 0;
        private int tripleCount = 0;

        public MainPage()
        {
            InitializeComponent();
            UpdateScores();
        }

        private void Option(int x)
        {
            playerToGetOut = x;
            if (playerToGetOut == 1)
            {
                player1Score = player3Score;
                UpdateScores();
            }
            else if (playerToGetOut == 2)
            {
                player2Score = player3Score;
                UpdateScores();
            }
            Navigation.PopModalAsync();
        }

        private void OnPlayersSelected(object sender, EventArgs e)
        {
            if (sender is Button button && int.TryParse(button.CommandParameter.ToString(), out int newPlayerCount))
            {
                if (newPlayerCount == 2 && numberOfPlayers == 3)
                {
                    var Page = new ContentPage
                    {

                        Content = new StackLayout
                        {
                            Padding = new Thickness(20),
                            VerticalOptions = LayoutOptions.Center,
                            HorizontalOptions = LayoutOptions.Center,
                            Children =
                    {
                        new Label
                        {
                            Text = "Gracz do usunięcia:",
                            FontSize = 20,
                            HorizontalOptions = LayoutOptions.Center
                        },
                        new Button
                        {
                            Text = "1",
                            Margin = 10,
                            Command = new Command(() => Option(1) )
                        },
                        new Button
                        {
                            Text = "2",
                            Margin = 10,
                            Command = new Command(() => Option(2) )
                        },
                        new Button
                        {
                            Text = "3",
                            Margin = 10,
                            Command = new Command(() => Option(3) )
                        }
                    }
                        }
                    };
                    Navigation.PushModalAsync(Page);
                }

                else if (newPlayerCount == 3 && numberOfPlayers == 2)
                {
                    AddPlayer();
                }
                numberOfPlayers = newPlayerCount;
                UpdateScores();

            }
        }

        private void AddPlayer()
        {
            player3Score = 501;
            Player3ScoreLabel.IsVisible = true;
            Player3ScoreLabel.Text = $"Player 3: {player3Score}";
        }

        private void UpdateScores()
        {
            Player1ScoreLabel.Text = $"Player 1: {player1Score}";

            Player2ScoreLabel.Text = $"Player 2: {player2Score}";
            Player3ScoreLabel.IsVisible = numberOfPlayers == 3;
            Player3ScoreLabel.Text = numberOfPlayers == 3 ? $"Player 3: {player3Score}" : "";

            Kogokolej.Text = isPlayer1Turn ? "Tura gracza 1" :
                              isPlayer2Turn ? "Tura gracza 2" :
                              isPlayer3Turn ? "Tura gracza 3" :
                              "Koniec gry";
            Obecnarunda.Text = "Punkty w tej rundzie: " + string.Join(", ", currentRoundPoints);
        }

        private void OnPointButtonClicked(object sender, EventArgs e)
        {
            if (currentRoundPoints.Count >= 3)
            {
                DisplayAlert("Błąd", "Masz tylko 3 rzuty na rundę.", "OK");
                return;
            }

            if (sender is Button button && int.TryParse(button.Text, out int points))
            {
                currentRoundPoints.Add(points);
                lastThrowModified.Add(false);
                lastThrowDoubled.Add(false);
                lastThrowTripled.Add(false);
                UpdateScores();
            }
        }

        private void OnGreenCenter(object sender, EventArgs e)
        {
            if (currentRoundPoints.Count < 3)
            {
                currentRoundPoints.Add(25);
                lastThrowModified.Add(false);
                lastThrowDoubled.Add(false);
                lastThrowTripled.Add(false);
                UpdateScores();
            }
            else
            {
                DisplayAlert("Błąd", "Masz tylko 3 rzuty na rundę.", "OK");
            }
        }

        private void OnRedCenter(object sender, EventArgs e)
        {
            if (currentRoundPoints.Count < 3)
            {
                currentRoundPoints.Add(50);
                lastThrowModified.Add(false);
                lastThrowDoubled.Add(false);
                lastThrowTripled.Add(false);
                UpdateScores();
            }
            else
            {
                DisplayAlert("Błąd", "Masz tylko 3 rzuty na rundę.", "OK");
            }
        }

        private void OnDoubleButtonClicked(object sender, EventArgs e)
        {
            if (currentRoundPoints.Count > 0 && !lastThrowModified.Last())
            {
                int lastIndex = currentRoundPoints.Count - 1;
                int lastPoint = currentRoundPoints[lastIndex];

                if (lastPoint > 0 && lastPoint != 25 && lastPoint != 50)
                {
                    int lastTIndex = lastThrowModified.Count - 1;
                    int lastTDIndex = lastThrowDoubled.Count - 1;

                    currentRoundPoints[lastIndex] = lastPoint * 2;
                    doubleCount = doubleCount + 1;
                    lastThrowDoubled[lastTDIndex] = true;
                    lastThrowModified[lastTIndex] = true;
                    UpdateScores();
                }
                else
                {
                    DisplayAlert("Błąd", "Nie można pomnożyć wartości środka.", "OK");
                }
            }
            else
            {
                DisplayAlert("Błąd", "Możesz modyfikować rzut tylko raz.", "OK");
            }
        }

        private void OnTripleButtonClicked(object sender, EventArgs e)
        {
            if (currentRoundPoints.Count > 0 && !lastThrowModified.Last())
            {
                int lastIndex = currentRoundPoints.Count - 1;
                int lastPoint = currentRoundPoints[lastIndex];

                if (lastPoint > 0 && lastPoint != 25 && lastPoint != 50)
                {
                    int lastTIndex = lastThrowModified.Count - 1;
                    int lastTTIndex = lastThrowTripled.Count - 1;
                    currentRoundPoints[lastIndex] = lastPoint * 3;
                    tripleCount = tripleCount + 1;
                    lastThrowTripled[lastTTIndex] = true;
                    lastThrowModified[lastTIndex] = true;
                    UpdateScores();
                }
                else
                {
                    DisplayAlert("Błąd", "Nie można pomnożyć wartości środka.", "OK");
                }
            }
            else
            {
                DisplayAlert("Błąd", "Możesz modyfikować rzut tylko raz.", "OK");
            }
        }

        private void OnRemoveLastThrowClicked(object sender, EventArgs e)
        {
            if (currentRoundPoints.Count > 0)
            {
                currentRoundPoints.RemoveAt(currentRoundPoints.Count - 1);
                lastThrowModified.RemoveAt(lastThrowModified.Count - 1);
                int LTDindex = lastThrowDoubled.Count - 1;
                int LTTindex = lastThrowTripled.Count - 1;
                if (lastThrowDoubled.Last())
                {
                    doubleCount = doubleCount - 1;

                }
                else if (lastThrowTripled.Last())
                {
                    tripleCount = tripleCount - 1;
                }
                lastThrowDoubled.RemoveAt(LTDindex);
                lastThrowTripled.RemoveAt(LTTindex);
                UpdateScores();
            }
        }

        private void OnConfirmButtonClicked(object sender, EventArgs e)
        {
            int roundScore = currentRoundPoints.Sum();
            int greenBullseyeCount = currentRoundPoints.Count(p => p == 25);
            int redBullseyeCount = currentRoundPoints.Count(p => p == 50);

            string message = "";
            if (doubleCount == 2) message = "Napij się pół kieliszka wody z popitą!";
            if (doubleCount == 3) message = "Napij się kieliszek wody z popitą!";
            if (doubleCount == 2 && tripleCount == 1) message = "Napij się kieliszek wody bez popity!";
            if (tripleCount == 2) message = "Napij się kieliszek wody z popitą!";
            if (tripleCount == 2 && doubleCount == 1) message = "Napij się kieliszek bez popity bez użycia rąk!";
            if (tripleCount == 3) message = "Napij się 2 kieliszki z popitą!";
            if (greenBullseyeCount > 0) message += "\nNapij się kieliszek z popitą za każdy zielony środek!";
            if (redBullseyeCount > 0) message += "\nNapij się kieliszek czystej za każdy czerwony środek!";

            doubleCount = 0;
            tripleCount = 0;

            if (!string.IsNullOrEmpty(message))
            {
                DisplayAlert("Zasada picia!", message, "OK");
            }

            bool scoreExceeded = false;

            if (isPlayer1Turn)
            {
                if (player1Score - roundScore < 0)
                {
                    scoreExceeded = true;
                }
                else
                {
                    player1Score -= roundScore;
                    if (player1Score == 0) player1Score = 0;
                }
            }
            else if (isPlayer2Turn)
            {
                if (player2Score - roundScore < 0)
                {
                    scoreExceeded = true;
                }
                else
                {
                    player2Score -= roundScore;
                    if (player2Score == 0) player2Score = 0;
                }
            }
            else if (isPlayer3Turn)
            {
                if (player3Score - roundScore < 0)
                {
                    scoreExceeded = true;
                }
                else
                {
                    player3Score -= roundScore;
                    if (player3Score == 0) player3Score = 0;
                }
            }

            if (scoreExceeded)
            {
                DisplayAlert("Zasada picia!", "Przebijasz wynik - napij się kieliszek bez popity!", "OK");
            }

            SwitchPlayerTurn();
        }

        private void SwitchPlayerTurn()
        {
            if (numberOfPlayers == 2)
            {
                isPlayer1Turn = !isPlayer1Turn;
                isPlayer2Turn = !isPlayer2Turn;
            }
            else if (numberOfPlayers == 3)
            {
                if (isPlayer1Turn)
                {
                    isPlayer1Turn = false;
                    isPlayer2Turn = false;
                    isPlayer3Turn = true;
                }
                else if (isPlayer2Turn)
                {
                    isPlayer1Turn = false;
                    isPlayer2Turn = false;
                    isPlayer3Turn = true;
                }
                else if (isPlayer3Turn)
                {
                    isPlayer1Turn = true;
                    isPlayer2Turn = false;
                    isPlayer3Turn = false;
                }
            }

            currentRoundPoints.Clear();
            lastThrowTripled.Clear();
            lastThrowModified.Clear();
            lastThrowDoubled.Clear();
            UpdateScores();
        }

        private void ResetGame(object sender, EventArgs e)
        {
            player1Score = 501;
            player2Score = 501;
            player3Score = 501;
            numberOfPlayers = 2;
            isPlayer1Turn = true;
            isPlayer2Turn = false;
            isPlayer3Turn = false;
            currentRoundPoints.Clear();
            UpdateScores();
        }
    }
}
