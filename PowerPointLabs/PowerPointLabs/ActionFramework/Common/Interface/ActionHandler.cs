using System;

namespace PowerPointLabs.ActionFramework.Common.Interface
{
    /// <summary>
    /// Handler that handles OnAction call
    /// </summary>
    public abstract class ActionHandler : BaseHandler
    {
        public void Execute(string ribbonId)
        {
            try
            {
                ExecuteAction(ribbonId);
            }
            catch (Exception e)
            {
                Log.Logger.LogException(e, ribbonId);
                Views.ErrorDialogBox.ShowDialog("PowerPointLabs", e.Message, e);
            }
        }

        protected abstract void ExecuteAction(string ribbonId);
    }
}
