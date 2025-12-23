// This code has been made by Simon VRANA.
// Please ask by email (simon.vrana.pro@gmail.com) before reusing for commercial purpose.

using Microsoft.Extensions.Logging;
using UnityEngine;

namespace Helyn.Logger.Example
{
	public class AnExampleClass : MonoBehaviour
	{
		// create a logger for this class (namespace + class name)
		private static readonly Microsoft.Extensions.Logging.ILogger helynLogger = LoggerSetup.LoggerFactory.CreateLogger<AnExampleClass>();

		public void SendExampleLogTrace()
		{
			helynLogger.LogTrace("This is an example of Trace log.");
		}

		public void SendExampleLogDebug()
		{
			helynLogger.LogDebug("This is an example of Debug log.");
		}

		public void SendExampleLogInformation()
		{
			helynLogger.LogInformation("This is an example of Information log.");
		}

		public void SendExampleLogWarning()
		{
			helynLogger.LogWarning("This is an example of Warning log.");
		}

		public void SendExampleLogError()
		{
			helynLogger.LogError("This is an example of Error log.");
		}

		public void SendExampleLogCritical()
		{
			helynLogger.LogCritical("This is an example of Critical log.");
		}

		public void SendExampleLogException()
		{
			try
			{
				throw new System.Exception("This is an example exception.");
			}
			catch (System.Exception ex)
			{
				helynLogger.LogError(ex, "An exception has been caught.");
			}
		}
	}
}
