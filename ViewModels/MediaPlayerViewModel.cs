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
        private int _maxLine;

        private int _playerPercent;

        private int _currentLine;

        private readonly IFlightSimulator model;
        public MediaPlayerViewModel()
        {
            this.model = simulator;
            this.model.PropertyChanged += delegate (object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_MediaPlayer_" + e.PropertyName);
                if (e.PropertyName == "CurrentLineNumber")
                {
                    NotifyPropertyChanged("VM_MediaPlayer_PlayPercent");
                }
            };
            _maxLine = getTotalFrameNumber();
        }

        public int VM_MediaPlayer_last_line
        {
            get
            {
                return _maxLine;
            }
        }

        public int VM_MediaPlayer_CurrentLineNumber
        {
            get
            {
                _currentLine = this.model.GetCurrentLine();
                return _currentLine;
            }
            set
            {
                this.model.SetCurrentLine(value);
                VM_MediaPlayer_PlayPercent = (int)((float)value / (float)_maxLine * 100);
                NotifyPropertyChanged("VM_MediaPlayer_CurrentLineNumber");
            }
        }

        public double VM_MediaPlayer_speed
        {
            get
            {
                return this.model.Speed;
            }
            set { }
        }


        public int VM_MediaPlayer_PlayPercent
        {
            get
            {
                _playerPercent = (int)((float)_currentLine / (float)_maxLine * 100);
                return _playerPercent;
            }
            set
            {
                _playerPercent = value;
                NotifyPropertyChanged("VM_MediaPlayer_PlayPercent");
            }
        }

        public int getTotalFrameNumber() { return simulator.GetNumLines(); }

        private void SetWithCheck(double speed)
        {
            if (speed < 0.2f)
            {
                Trace.WriteLine($"cannot update speed to {speed} - speed too low");
                return;
            }
            else if (speed > 32.0f)
            {
                Trace.WriteLine($"cannot update speed to {speed} - speed too high");
                return;
            }
            this.model.SetSpeed(speed);
        }
        public void pause()
        {
            this.model.pauseRun();
        }
        public void play()
        {
            this.model.SetSpeed(1.0f);
            this.model.SetForward(true);

            this.model.unPauseRun();
        }
        public void stop()
        {
            this.model.pauseRun();
            this.model.SetCurrentLine(0);
        }
        public void startOverLines()
        {
            this.model.pauseRun();
            this.model.SetCurrentLine(0);
        }
        public void finishLines()
        {
            this.model.pauseRun();
            this.model.SetCurrentLine(getTotalFrameNumber() - 1);
        }
        public void moveToLine(int x)
        {
            this.model.SetCurrentLine(x);
        }
        public void updatePlaySpeed(String s)
        {
            float f;
            if (float.TryParse(s, out f))
            {
                this.model.SetSpeed(f);
            }
        }

        public void updateSpeed(float value)
        {
            SetWithCheck(this.model.Speed + value);
        }
        
        public void forward()
        {
            if (this.model.GetForward())
            {
                SetWithCheck(this.model.Speed * 2);
            }
            else
            {
                this.model.SetForward(true);
                this.model.SetSpeed(2.0f);
            }
        }
        public void rewind()
        {
            if (!this.model.GetForward())
            {
                SetWithCheck(this.model.Speed * 2);
            }
            else
            {
                this.model.SetForward(false);
                this.model.SetSpeed(2.0f);
            }
        }
    }
}
