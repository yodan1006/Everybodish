using System;
using UnityEngine;

//This breaks direct access to UnityEngine.Debug on purpose
namespace DebugBehaviour.Runtime
{
    public class VerboseMonoBehaviour : MonoBehaviour
    {
        public bool isVerbose = true;

        protected void Log(string msg)
        {
            if (isVerbose)
            {
                Debug.Log(msg, this);
            }
        }

        protected void Log(UnityEngine.Object message)
        {
            if (isVerbose)
            {
                Debug.Log(LogType.Log, message);
            }
        }

        protected void Log(object message, UnityEngine.Object context)
        {
            if (isVerbose)
            {
                Debug.Log(message, context);
            }
        }

        protected void LogFormat(string format, params object[] args)
        {
            if (isVerbose)
            {
                Debug.LogFormat(format, args);
            }
        }
        protected void LogFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (isVerbose)
            {
                Debug.LogFormat(context, format, args);
            }
        }

        protected void LogFormat(LogType logType, LogOption logOptions, UnityEngine.Object context, string format, params object[] args)
        {
            if (isVerbose)
            {
                Debug.LogFormat(logType, logOptions, context, format, args);
            }
        }

        protected void LogError(UnityEngine.Object message)
        {
            if (isVerbose)
            {
                Debug.LogError(LogType.Error, message);
            }
        }

        protected void LogError(UnityEngine.Object message, UnityEngine.Object context)
        {
            if (isVerbose)
            {
                Debug.Log(message, context);
            }
        }

        protected void LogErrorFormat(string format, params object[] args)
        {
            if (isVerbose)
            {
                Debug.LogFormat(format, format, args);
            }
        }

        protected void LogErrorFormat(UnityEngine.Object context, string format, params object[] args)
        {
            if (isVerbose)
            {
                Debug.LogFormat(context, format, args);
            }
        }


        protected void ClearDeveloperConsole()
        {
            Debug.ClearDeveloperConsole();
        }

        protected void LogException(Exception exception)
        {
            if (isVerbose)
            {
                Debug.LogException(exception, null);
            }
        }

        protected void LogException(Exception exception, UnityEngine.Object context)
        {
            if (isVerbose)
            {
                Debug.LogException(exception, context);
            }
        }

        protected void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
        {
            if (isVerbose)
            {
                Debug.DrawLine(start, end, color, duration);
            }
        }

        protected void DrawLine(Vector3 start, Vector3 end, Color color)
        {
            if (isVerbose)
            {
                Debug.DrawLine(start, end, color);
            }
        }

        protected void DrawLine(Vector3 start, Vector3 end)
        {
            if (isVerbose)
            {
                Debug.DrawLine(start, end);
            }
        }

        protected void DrawLine(Vector3 start, Vector3 end, [UnityEngine.Internal.DefaultValue("Color.white")] Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest)
        {
            if (isVerbose)
            {
                Debug.DrawLine(start, end, color, duration, depthTest);
            }
        }

        protected void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        {
            if (isVerbose)
            {
                Debug.DrawRay(start, dir, color, duration);
            }
        }

        protected void DrawRay(Vector3 start, Vector3 dir, Color color)
        {
            if (isVerbose)
            {
                Debug.DrawRay(start, dir, color);
            }
        }


        protected void DrawRay(Vector3 start, Vector3 dir)
        {
            if (isVerbose)
            {
                Debug.DrawRay(start, dir);
            }
        }

        protected void DrawRay(Vector3 start, Vector3 dir, [UnityEngine.Internal.DefaultValue("Color.white")] Color color, [UnityEngine.Internal.DefaultValue("0.0f")] float duration, [UnityEngine.Internal.DefaultValue("true")] bool depthTest)
        {
            if (isVerbose)
            {
                Debug.DrawLine(start, start + dir, color, duration, depthTest);
            }
        }


        protected void Break()
        {
            if (isVerbose)
            {
                Debug.Break();
            }
        }

        protected void DebugBreak()
        {
            if (isVerbose)
            {
                Debug.DebugBreak();
            }
        }
    }
}
