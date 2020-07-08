using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueLock {
	class Options {

		public bool TurnOffOnLock { get; set; }
		public bool TurnOffOnShutdown { get; set; } // TODO: turn off, retain current color, use some preconfigured color

	}
}
