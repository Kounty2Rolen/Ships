//BattleShips Copyright (C) 2020 Daniil Molodtsov (molodcovdaniil123@gmail.com)
/*
Это программа является свободным программным обеспечением. Вы можете распространять и/или модифицировать её согласно условиям Стандартной Общественной Лицензии GNU, опубликованной Фондом Свободного Программного Обеспечения, версии 3 или, по Вашему желанию, любой более поздней версии. Эта программа распространяется в надежде, что она будет полезной, но БЕЗ ВСЯКИХ ГАРАНТИЙ, в том числе подразумеваемых гарантий ТОВАРНОГО СОСТОЯНИЯ ПРИ ПРОДАЖЕ и ГОДНОСТИ ДЛЯ ОПРЕДЕЛЁННОГО ПРИМЕНЕНИЯ. Смотрите Стандартную Общественную Лицензию GNU для получения дополнительной информации. Вы должны были получить копию Стандартной Общественной Лицензии GNU вместе с программой. В случае её отсутствия, посмотрите <http://www.gnu.org/licenses/>. 
*/
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace BattleShips
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Ships.Content = 20.ToString();
        }

        private static bool[,] P1Field = new bool[10, 10];
        private static bool[,] P2Field;

        private void initbot()
        {
            P2Field = new bool[10, 10];
            Random random = new Random((new Random().Next(0, 99999) * DateTime.Now.Hour) / new Random().Next(1, 10));
            var ships = 20;
            while (ships != 0)
            {
                var x = random.Next(0, 9);
                var y = random.Next(0, 9);
                if (P2Field[x, y] == false)
                {
                    P2Field[x, y] = true;
                    ships -= 1;
                }
            }
        }

        private void botTurn(Button button=null)
        {
            P2Lock.Fill = Brushes.Transparent;
            Random random = new Random((new Random().Next(0, 99999) * DateTime.Now.Hour) / new Random().Next(1, 10));
            var ships = Convert.ToInt32(ScoreP2.Content);
            var x = random.Next(0, 9);
            var y = random.Next(0, 9);
            Button _btn = new Button();
            int xP = 0;
            int yP = 0;
            if (button == null)
            {
                foreach (var item in Player1Grid.Children)
                {
                    if (item.GetType() == typeof(Button))
                    {
                        var btn = item as Button;
                        xP = Convert.ToInt32(btn.Tag.ToString().Split(',')[0]);
                        yP = Convert.ToInt32(btn.Tag.ToString().Split(',')[1]);
                        if (x == xP && y == yP)
                        {
                            _btn = btn;
                            break;
                        }
                    }
                }
            }
            else {
                _btn = button;
            }
            if (_btn.Background == Brushes.Red || _btn.Background == Brushes.Blue)
            {
                botTurn(_btn);
            }
            if (ships != 20)
            {
                if (P1Field[x, y] == true)
                {
                    if (x == xP && y == yP)
                    {
                        _btn.Background = Brushes.Red;
                        ships -= 1;
                        ScoreP2.Content = Convert.ToInt32(ScoreP2.Content) + 1;
                    }
                }
                else
                {
                    if (x == xP && y == yP)
                    {
                        _btn.Background = Brushes.Blue;
                    }
                }
            }
        }

        private void retry()
        {
            ScoreP1.Content = 0;
            ScoreP2.Content = 0;
            StartBtn.Content = "Start";
            P2Lock.Fill = Brushes.Transparent;
            Ships.Content = 20;
            foreach (var item in Player2Grid.Children)
            {
                if (item.GetType() == typeof(Button))
                {
                    var _btn = item as Button;
                    _btn.Background = StartBtn.Background;
                }
            }
            foreach (var item in Player1Grid.Children)
            {
                if (item.GetType() == typeof(Button))
                {
                    var _btn = item as Button;
                    _btn.Background = StartBtn.Background;
                }
            }
            P1Field = new bool[9, 9];
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var ships = Convert.ToInt32(Ships.Content);
            var _btn = sender as Button;

            if (_btn.Background != Brushes.DarkKhaki)
            {
                if (ships > 0)
                {
                    _btn.Background = Brushes.DarkKhaki;
                    var x = Convert.ToInt32(_btn.Tag.ToString().Split(',')[0]);
                    var y = Convert.ToInt32(_btn.Tag.ToString().Split(',')[1]);
                    P1Field[x, y] = true;
                    Ships.Content = ships - 1;
                }
            }
            else
            {
                var x = Convert.ToInt32(_btn.Tag.ToString().Split(',')[0]);
                var y = Convert.ToInt32(_btn.Tag.ToString().Split(',')[1]);
                if (P1Field[x, y] == true)
                {
                    _btn.Background = StartBtn.Background;
                    P1Field[x, y] = false;
                    Ships.Content = ships + 1;
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var _btn = sender as Button;
            if (_btn.Background == Brushes.Red || _btn.Background == Brushes.Blue)
            {
                return;
            }
            var x = Convert.ToInt32(_btn.Tag.ToString().Split(',')[0]);
            var y = Convert.ToInt32(_btn.Tag.ToString().Split(',')[1]);
            if (P2Field[x, y] == true)
            {
                _btn.Background = Brushes.Red;
                ScoreP1.Content = Convert.ToInt32(ScoreP1.Content) + 1;
            }
            else
            {
                _btn.Background = Brushes.Blue;
            }

            botTurn();
            if (Convert.ToInt32(ScoreP1.Content) == 20)
            {
                MessageBox.Show("You won!", "WOW", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                if (MessageBox.Show("Retry?", "?!?!?!?!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    retry();
                    return;
                }
                else
                {
                    StartBtn.Content = "Retry?";
                    return;
                }
            }
            if (Convert.ToInt32(ScoreP2.Content) == 20)
            {
                if (MessageBox.Show("You lose!\nRetry?", "?!?!?!?!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    retry();
                    return;
                }
                else
                {
                    StartBtn.Content = "Retry?";
                    StartBtn.IsEnabled = true;
                    return;
                }
            }
            P2Lock.Fill = null;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if ((string)StartBtn.Content == "Retry?")
            {
                retry();
            }
            var ships = Convert.ToInt32(Ships.Content);
            if (ships > 0)
            {
                MessageBox.Show("Not all sips are placed", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                P2Lock.Fill = null;
                StartBtn.IsEnabled = false;
                initbot();
            }
        }
    }
}