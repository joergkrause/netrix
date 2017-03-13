namespace GuruComponents.Netrix.Designer
{
    /// <summary>
    /// TODO: Add comment.
    /// </summary>
    public class DirectiveEventArgs
    {
        private IDirective _directive;


        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public IDirective Directive
        {
            get
            {
                return _directive;
            }
        }

        /// <summary>
        /// TODO: Add comment.
        /// </summary>
        public DirectiveEventArgs(IDirective directive)
        {
            _directive = directive;
        }
    }

}
