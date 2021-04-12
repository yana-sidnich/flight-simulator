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
using System.Diagnostics;

namespace FlightGearTestExec.Controls
{
    /// <summary>
    /// Interaction logic for MediaPlayerView.xaml
    /// </summary>
    public partial class MediaPlayerView : UserControl
    {

        private MediaPlayerViewModel vm;
        public int maxLine;
        public MediaPlayerView()
        {
            InitializeComponent();
            this.vm = new MediaPlayerViewModel();

            currentFrameSlider.Maximum = vm.getTotalFrameNumber();
            currentFrameSlider.Minimum = 0;
            maxLine = vm.getTotalFrameNumber();
            this.DataContext = this.vm;
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
            vm.rewind();
        }

        private void forwardButton_Click(object sender, RoutedEventArgs e)
        {
            vm.forward();
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            vm.increaseSpeed();

        }

        private void PlusTimesFive_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                vm.increaseSpeed();
            }            

        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            vm.decreaseSpeed();

        }

        private void MinusTimesFive_Click(object sender, RoutedEventArgs e)
        {
            for (int i = 0; i < 5; i++)
            {
                vm.decreaseSpeed();
            }

        }
    }



}
