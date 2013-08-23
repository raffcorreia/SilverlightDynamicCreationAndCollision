using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace DynamicCreationAndCollision
{
	public partial class Ball : UserControl
	{
		private TextBox painelTop;
		private TextBox painelLeft;
		private double rootHeight;
		public double Speed;
		private bool monitoring;
		
		public double Left 	    { get { return Canvas.GetLeft(this); } }
		public double Right 	{ get { return Canvas.GetLeft(this) + this.Width ; } }
		public double Top       { get { return Canvas.GetTop(this); } }
		public double Bottom    { get { return Canvas.GetTop(this) + this.Height ; } }
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
					ball.Stroke = new SolidColorBrush(Color.FromArgb(255, 255, 0, 0));
				}
				else
				{
					ball.Stroke = null;
				}				
				if(MonitoringChanged != null)
					MonitoringChanged(this, new MonitoringArgs(value));
			}
		}
		
		public Ball(double Height, ref TextBox txtTop, ref TextBox txtLeft)
		{
			this.InitializeComponent();
			
			this.rootHeight = Height;
			this.painelTop = txtTop;
			this.painelLeft = txtLeft;
			this.Speed = (new Random(DateTime.Now.Millisecond).NextDouble()) * 2.0;
			this.monitoring = false;
         	
			Move.Completed += new EventHandler(Move_Completed);
            Move.Begin();
        }
		
		public event MonitoringHaldler MonitoringChanged;		

        private void Move_Completed(object sender, EventArgs e)
        {
            Canvas.SetTop(this, Top + Speed);

			if (this.Monitoring)
			{
				painelTop.Text = Top.ToString("0.00");
				painelLeft.Text = Left.ToString("0.00");
			}
			
            if (Top + this.Height >= this.rootHeight)
            {
                Speed *= -1;
            }
            else if (Top <= 0)
            {
                Speed *= -1;
            }
            Move.Begin();
        }

		private void Ellipse_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
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
		}
	}
}