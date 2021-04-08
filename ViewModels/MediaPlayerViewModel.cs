using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightGearTestExec;

namespace FlightGearTestExec.ViewModels
{
    class MediaPlayerViewModel : INotifyPropertyChanged
    {

        private IFlightSimulator simulator;
        public event PropertyChangedEventHandler PropertyChanged;
        
        public MediaPlayerViewModel(IFlightSimulator simulator)
        {
            this.simulator = simulator;
            this.simulator.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_media_player_" + e.PropertyName);
            };
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        int vm_media_player_current_line
        {
            get { return simulator.GetCurrentLine(); }
            set { simulator.SetCurrentLine(value); }
        }
        public int getTotalFrameNumber() { return simulator.GetNumLines(); }
        public void pause()
        {
            simulator.pauseRun();
        }
        public void play()
        {
            simulator.unPauseRun();
        }
        public void stop()
        {
            simulator.pauseRun();
            simulator.SetCurrentLine(0);
        }
        public void startOverLines()
        {
            simulator.SetCurrentLine(0);
        }
        public void finishLines()
        {
            simulator.SetCurrentLine(getTotalFrameNumber() - 1);
        }
        public void moveToLine(int x)
        {
            simulator.SetCurrentLine(x);
        }
        public void updatePlaySpeed(String s)
        {
            float f;
            if (float.TryParse(s, out f))
            {
                simulator.SetSpeed(f);
            }
        }

        public void increaseSpeed()
        {
            simulator.SetSpeed(simulator.GetSpeed() + 0.1f);
        }
        public void decreaseSpeed()
        {
            if (simulator.GetSpeed() > 0.2f)
                simulator.SetSpeed(simulator.GetSpeed() - 0.1f);
        }
        public void halfSpeed()
        {
                simulator.SetSpeed(simulator.GetSpeed() / 2);
        }
        public void doubleSpeed()
        {
            simulator.SetSpeed(simulator.GetSpeed() * 2);
        }
        public float getSpeed()
        {
            return simulator.GetSpeed();
        }
    }
}
