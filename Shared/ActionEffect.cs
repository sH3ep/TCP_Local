namespace TPC.Api.Shared
{
    public class ActionEffect
    {
        private bool _isSuccesfull;
        private string _errorMessage;

        public ActionEffect()
        {
            _isSuccesfull = true;
        }

        public ActionEffect(string errorMessage)
        {
            _isSuccesfull = false;
            _errorMessage = errorMessage;
        }

        public ActionEffect(bool isSuccesfull,string errorMessage)
        {
            _isSuccesfull = isSuccesfull;
            _errorMessage = errorMessage;
        }

        public bool IsSuccesfull
        {
            private set => _isSuccesfull = value;
            get => _isSuccesfull;
        }

        public string ErrorMessage
        {
            private set => _errorMessage = value;
            get => _errorMessage;
        }
    }
}
