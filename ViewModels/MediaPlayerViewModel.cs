using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightGearTestExec;
using System.Diagnostics;
using System.Windows.Data;
using FlightGearTestExec.Converters;

namespace FlightGearTestExec.ViewModels
{
    class MediaPlayerViewModel : BaseViewModel
    {

        
        public MediaPlayerViewModel()
        {
            this.simulator.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_media_player_" + e.PropertyName);
            };
        }

        public int vm_media_player_current_line
        {
            get {
                Trace.WriteLine("get current line");
                return simulator.GetCurrentLine(); }
            set { simulator.SetCurrentLine(value);
                }
        }

        public double vm_media_player_speed
        {
            get
            {
                return simulator.GetSpeed();
            }
            set { }
        }
        public int getTotalFrameNumber() { return simulator.GetNumLines(); }
        public void pause()
        {
            simulator.pauseRun();
        }
        public void play()
        {
            simulator.SetSpeed(1.0f);
            simulator.SetForward(true);

            simulator.unPauseRun();
        }
        public void stop()
        {
            simulator.pauseRun();
            simulator.SetCurrentLine(0);
        }
        public void startOverLines()
        {
            simulator.pauseRun();
            simulator.SetCurrentLine(0);
        }
        public void finishLines()
        {
            simulator.pauseRun();
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
        public void forward()
        {
            if (simulator.GetForward())
            {
                simulator.SetSpeed(simulator.GetSpeed() * 2);
            }
            else
            {
                simulator.SetForward(true);
                simulator.SetSpeed(2.0f);
            }
        }
        public void rewind()
        {
            if (!simulator.GetForward())
            {
                simulator.SetSpeed(simulator.GetSpeed() * 2);
            }
            else
            {
                simulator.SetForward(false);
                simulator.SetSpeed(2.0f);
            }
        }
        public double GetSpeed()
        {
            return simulator.GetSpeed();
        }
    }
}
