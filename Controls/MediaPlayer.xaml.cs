using FlightGearTestExec.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightGearTestExec.Controls
{
    /// <summary>
    /// Interaction logic for MediaPlayer.xaml
    /// </summary>
    public partial class MediaPlayer : UserControl
    {
            private MediaPlayerViewModel vm;
            public MediaPlayer()
            {
                this.vm = new MediaPlayerViewModel(FlightSimuatorSingleton.simulator);
                InitializeComponent();
                this.DataContext = vm;

                currentFrameSlider.Maximum = vm.getTotalFrameNumber();
                currentFrameSlider.Minimum = 0;
            }


            private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
            {

               // int x = (int)((Slider)sender).Value; // it will be int because of IsSnapToTick is true.
               // vm.moveToLine(x);
               // if (x % 60 >= 10)
               // {
               //     currentFrameLabel.Content = "" + x / 60 + ":" + x % 60;
               // }
               // else
               // {
               //     currentFrameLabel.Content = "" + x / 60 + ":0" + x % 60;
               // }

            }
               private void TextBox_TextChanged(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                TextBox tx = (TextBox)sender;
                vm.updatePlaySpeed(tx.Text);
            }
        }

            private void pauseButton_Click(object sender, RoutedEventArgs e)
            {
                vm.pause();
            }

            private void playButton_Click(object sender, RoutedEventArgs e)
            {
                vm.play();
            }

            private void stopButton_Click(object sender, RoutedEventArgs e)
            {
                vm.stop();
            }

            private void weirdBackwardButton_Click(object sender, RoutedEventArgs e)
            {
                vm.startOverLines();
            }

            private void weirdForwardButton_Click(object sender, RoutedEventArgs e)
            {
                vm.finishLines();
            }

            private void backwardButton_Click(object sender, RoutedEventArgs e)
            {
                vm.decreaseSpeed();
                speedTxt.Text = vm.getSpeed().ToString();
            }

            private void forwardButton_Click(object sender, RoutedEventArgs e)
            {
                vm.increaseSpeed();
                speedTxt.Text = vm.getSpeed().ToString();
            }

            private void Plus_Click(object sender, RoutedEventArgs e)
            {
                vm.increaseSpeed();
                speedTxt.Text = vm.getSpeed().ToString();

        }
        private void Minus_Click(object sender, RoutedEventArgs e)
            {
            speedTxt.Text = vm.getSpeed().ToString();
        }
         }
    }
