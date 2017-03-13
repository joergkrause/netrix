namespace GuruComponents.Netrix.WebEditing.HighLighting
{
    /// <summary>
    /// Controls the movement of text pointers.
    /// </summary>
    public enum MoveTextPointer
    {
        /// <summary>
        /// Move the pointer one character backwards.
        /// </summary>
        PrevChar = 0,
        /// <summary>
        /// Move the pointer one character forward.
        /// </summary>
        NextChar = 1,
        /// <summary>
        /// Move the pointer to the beginning of the previous cluster.
        /// </summary>
        PrevClusterBegin = 2,
        /// <summary>
        /// Move the pointer to the beginning of the next cluster.
        /// </summary>
        NextClusterBegin = 3,
        /// <summary>
        /// Move the pointer to the end of the previous cluster.
        /// </summary>
        PrevClusterEnd = 4,
        /// <summary>
        /// Move the pointer to the end of the next cluster.
        /// </summary>
        NextClusterEnd = 5,
        /// <summary>
        /// Move the pointer to the beginning of the previous word.
        /// </summary>
        PrevWordBegin = 6,
        /// <summary>
        /// Move the pointer to the beginning of the next word.
        /// </summary>
        NextWordBegin = 7,
        /// <summary>
        /// Move the pointer to the end of the previous word.
        /// </summary>
        PrevWordEnd = 8,
        /// <summary>
        /// Move the pointer to the end of the next word.
        /// </summary>
        NextWordEnd = 9,
        /// <summary>
        /// Move the pointer to the previous word to proof.
        /// </summary>
        PrevProofWord = 10,
        /// <summary>
        /// Move the pointer to next word to proof.
        /// </summary>
        NextProofWord = 11,
        /// <summary>
        /// Move the pointer to the beginning of the next URL.
        /// </summary>
        NextUrlBegin = 12,
        /// <summary>
        /// Move the pointer to the beginning of the previous URL.
        /// </summary>
        PrevUrlBegin = 13,
        /// <summary>
        /// Move the pointer to the end of the next URL.
        /// </summary>
        NextUrlEnd = 14,
        /// <summary>
        /// Move the pointer to the end of the previous URL.
        /// </summary>
        PrevUrlEnd = 15,
        /// <summary>
        /// Move the pointer to the previous sentence.
        /// </summary>
        PrevSentence = 16,
        /// <summary>
        /// Move the pointer to the next sentence.
        /// </summary>
        NextSentence = 17,
        /// <summary>
        /// Move the pointer to the previous block.
        /// </summary>
        PrevBlock = 18,
        /// <summary>
        /// Move the pointer to the next block.
        /// </summary>
        NextBlock = 19,
    }

}