using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Collections.Generic;

namespace DynamicCreationAndCollision
{
	public partial class Catcher : UserControl
	{
		private Dictionary<int, Ball> balls;
		private TextBox painelTop;
		private TextBox painelLeft;
		private double rootWidth;
		public double Speed;
		private bool monitoring;
		public bool Monitoring
		{
			get 
			{
				return monitoring;
			}
			set 
			{
				monitoring = value;
				if (value)
				{
                    catcher.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
				}
				else
				{
                    catcher.Stroke = null;
				}				
				if(MonitoringChanged != null)
					MonitoringChanged(this, new MonitoringArgs(value));
			}
		}
		public double Left 	    { get { return Canvas.GetLeft(this); } }
		public double Right 	{ get { return Canvas.GetLeft(this) + this.Width ; } }
		public double Top 		{ get { return Canvas.GetTop(this); } }
		public double Bottom 	{ get { return Canvas.GetTop(this) + this.Height ; } }

		public event MonitoringHaldler MonitoringChanged;
		public event CaughtHaldler Caught;
		
		public Catcher(double Width, ref TextBox txtTop, ref TextBox txtLeft, ref Dictionary<int, Ball> ballDic)
		{
			// Required to initialize variables
			InitializeComponent();
			
			this.rootWidth = Width;
			this.painelTop = txtTop;
			this.painelLeft = txtLeft;
			this.Speed = (((new Random(DateTime.Now.Millisecond).NextDouble()) * 4.0) -2);
			this.monitoring = false;
			this.balls = ballDic;
         	
			Move.Completed += new EventHandler(Move_Completed);
            Move.Begin();
		}
		
		private void Cach()
		{
			foreach(KeyValuePair<int, Ball> b in balls)
			{
				//Test if they are in the same horizontal direction
				if(b.Value.Left >= this.Left && b.Value.Left <= this.Right 
					|| b.Value.Right >= this.Left && b.Value.Right <= this.Right )
				{
                    //Test if they are in the same vertical direction
					if(b.Value.Top >= this.Top && b.Value.Top <= this.Bottom 
						|| b.Value.Bottom >= this.Top && b.Value.Bottom <= this.Bottom )
					{
						balls.Remove(b.Key);
						b.Value.Finish();
						if(Caught != null)
							Caught(this, null);
						return;
					}
				}
			}
		}
		
        private void Move_Completed(object sender, EventArgs e)
        {
            Canvas.SetLeft(this, Left + Speed);
			Cach();
			if (this.monitoring)
			{
				painelTop.Text = Top.ToString("0.00");
				painelLeft.Text = Left.ToString("0.00");
			}
			
            if (Left + this.Width >= this.rootWidth)
            {
                Speed *= -1;
            }
            else if (Left <= 0)
            {
                Speed *= -1;
            }
            Move.Begin();
        }

		private void Catcher_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
		{
			this.Monitoring = !this.Monitoring;
		}
		
		public void Finish()
		{
			if (Move != null)
				Move.Stop();
			Canvas c = (Canvas)this.Parent;
			if (c != null)
				c.Children.Remove(this);
			this.balls = null;
		}
	}
	
	public delegate void CaughtHaldler(object sender, EventArgs e);
	
}