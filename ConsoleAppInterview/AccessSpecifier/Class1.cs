namespace AccessSpecifier
{
    internal class BaseClass // This class is accessible only within the same assembly
    {
        private class PrivateClass // This class is accessible only within the same class
        {
        }
        protected class ProtectedClass // This class is accessible within the same class and derived classes
        {
        }
        public class PublicClass // This class is accessible from any other code
        {
        }
        internal class InternalClass // This class is accessible only within the same assembly
        {
        }
        protected internal class ProtectedInternalClass // This can be accessed within the same assembly or derived classes
        {
        }
    }
   
}
