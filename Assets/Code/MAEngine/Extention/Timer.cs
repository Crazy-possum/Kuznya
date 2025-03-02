using System;
using UnityEngine;

namespace MAEngine.Extention
{
    public class Timer
    {
        public bool IsWaiting;
        private float _startWaitTime;
        private float _duration;

        public Timer()
        {
            IsWaiting = false;
            _startWaitTime = Time.time;
        }
        public Timer(float duration)
        {
            IsWaiting = true;
            _startWaitTime = Time.time;
            _duration = duration;
        }
        public Timer(float duration, bool withDelay = true)
        {
            if (withDelay)
            {
                IsWaiting = true;
                _startWaitTime = Time.time;
                _duration = duration;
            }
            else
            {
                IsWaiting = false;
                _startWaitTime = Time.time - duration;
                _duration = duration;
            }

        }

        public bool Wait()
        {
            float nextAvailableTime = _startWaitTime + _duration;
            if (Time.time > nextAvailableTime)
            {
                _startWaitTime = Time.time;
                IsWaiting = false;
                return true;
            }

            return false;
        }

        public float GetRemainingTime()
        {
            return (_startWaitTime + _duration) - Time.time;
        }

        public float GetRoundedRemainingTime(int numbersAfterDot)
        {
            return (float)Math.Round(GetRemainingTime(), numbersAfterDot);
        }
    }
}