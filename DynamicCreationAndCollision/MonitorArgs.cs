using System;

namespace DynamicCreationAndCollision
{
	public delegate void MonitorandoHaldler(object sender, MonitorandoArgs e);
	
	public class MonitorandoArgs : EventArgs {
		public bool valor;
		
		public MonitorandoArgs(bool val)
		{
			this.valor = val;
		}
	}
}