using System;

namespace DynamicCreationAndCollision
{
	public delegate void MonitoringHaldler(object sender, MonitoringArgs e);
	
	public class MonitoringArgs : EventArgs {
		public bool valor;
		
		public MonitoringArgs(bool val)
		{
			this.valor = val;
		}
	}
}