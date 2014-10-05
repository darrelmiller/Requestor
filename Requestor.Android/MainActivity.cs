using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace Requestor.Android
{
    [Activity(Label = "Requestor.Android", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        int count = 1;
        private ClientState _clientState;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            _clientState = new ClientState();
            _clientState.PropertyChanged += _clientState_PropertyChanged;
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.myButton);
            EditText target = FindViewById<EditText>(Resource.Id.Target);
            TextView response = FindViewById<TextView>(Resource.Id.Response);
            TextView status = FindViewById<TextView>(Resource.Id.Status);

            button.Click += async delegate
            {
                _clientState.CurrentTarget = target.Text;
                await _clientState.GoAsync();
				RunOnUiThread( delegate {
					response.Text = _clientState.CurrentResponse;
					status.Text = _clientState.CurrentStatus;
				});
                
            };
        }

        void _clientState_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            TextView status = FindViewById<TextView>(Resource.Id.Status);
            switch (e.PropertyName)
            {
			case "CurrentStatus":
				RunOnUiThread (delegate {
					status.Text = _clientState.CurrentStatus;
				});
                    break;
            }
        }
    }
}

