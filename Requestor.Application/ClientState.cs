using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Requestor
{
    public class ClientState  : INotifyPropertyChanged  
    {
        public string CurrentTarget
        {
            get { return _currentTarget; }
            set
            {
                _currentTarget = value;
                OnPropertyChanged();
            }
        }

        public string CurrentResponse
        {
            get { return _currentResponse; }
            set
            {
                _currentResponse = value;
                OnPropertyChanged();
            }
        }

        public string CurrentStatus
        {
            get { return _currentStatus; }
            set
            {
                _currentStatus = value; 
                OnPropertyChanged();
            }
        }

        private HttpClient _HttpClient;
        private string _currentResponse;
        private string _currentTarget;
        private string _currentStatus;

        public ClientState()
        {
            _HttpClient = new HttpClient();
			_HttpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("TavisRequestor","1.0"));
        }

        public async Task GoAsync()
        {
            CurrentStatus = "Making request to " + CurrentTarget;
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            try
            {
                    var requestTask = _HttpClient.GetAsync(CurrentTarget);

                    CurrentStatus = "Waiting for response from " + CurrentTarget;
                    var response = await requestTask;

                    CurrentStatus = "Parsing response";
                    var sb = new StringBuilder();
                    sb.AppendLine(string.Format("{0} {1}", (int)response.StatusCode, response.ReasonPhrase));

                    foreach (var header in response.Headers)
                    {
                        foreach (var headerValue in header.Value)
                        {
                            sb.AppendLine(String.Format("{0}: {1}", header.Key, headerValue));
                        }
                    }

                    sb.AppendLine();

                    if (response.Content != null)
                    {
                        foreach (var header in response.Content.Headers)
                        {
                            foreach (var headerValue in header.Value)
                            {
                                sb.AppendLine(String.Format("{0}: {1}", header.Key, headerValue));
                            }
                        }
                        sb.AppendLine();

                        sb.Append(await response.Content.ReadAsStringAsync());
                    }
                    CurrentResponse = sb.ToString();
                    stopwatch.Stop();
               
                CurrentStatus = "Request completed in " +stopwatch.ElapsedMilliseconds + " ms";
            }
            catch (Exception ex)
            {
                CurrentResponse = "Failed to make request : " +Environment.NewLine + ex.Message;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
