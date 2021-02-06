using Rublon.Sdk.Core.Exception;

namespace Rublon.Sdk.Core
{
    public  abstract class RublonHttpCallback
    {
        /// <summary>
        /// State GET parameter name.
        /// </summary>
        public const string PARAMETER_STATE = "rublonState";

        /// <summary>
        /// Access token GET parameter name.
        /// </summary>
        public const string PARAMETER_ACCESS_TOKEN = "rublonToken";

        /// <summary>
        /// Success state value.
        /// </summary>
        public const string STATE_OK = "ok";

        /// <summary>
        /// Error state value.
        /// </summary>
        public const string STATE_ERROR = "error";

        protected RublonConsumer rublon;

        /// <summary>
        /// Constructor.
        /// </summary>
        public RublonHttpCallback(RublonConsumer rublon)
        {
            rublon.TestConfiguration();
            this.rublon = rublon;
        }

        /// <summary>
        /// Invoke the callback.
        /// </summary>
        public virtual void Call()
        {
            var state = this.getState().ToLower();
            if (state == STATE_OK)
            {
                this.finalizeTransaction();
            }
            else if (state == STATE_ERROR)
            {
                this.handleError();
            }
            else
            {
                handleCancel();
            }
        }

        protected virtual void finalizeTransaction()
        {
            var token = getAccessToken();
            if (string.IsNullOrEmpty(token))
            {
                throw new CallbackException("Missing access token.");
            }
        }

        
        /// <summary>
        /// Handle authentication success.
        /// </summary>
        /// <param name="profileId"></param>
        protected abstract void userAuthenticated(string profileId);

        /// <summary>
        /// Handle state "error".
        /// </summary>
        protected abstract void handleError();

        /// <summary>
        /// Handle state "cancel".
        /// </summary>
        protected abstract void handleCancel();

        /// <summary>
        /// Get state from HTTP GET parameters or NULL if not present.
        /// </summary>
        /// <returns>string|NULL</returns>
        protected abstract string getState();

        /// <summary>
        /// Get access token from HTTP GET parameters or NULL if not present.
        /// </summary>
        /// <returns></returns>
        protected abstract string getAccessToken();
    }
}
