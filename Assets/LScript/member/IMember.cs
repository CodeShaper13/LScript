using UnityEngine;
using System.Collections;

namespace LScript {

    public interface IMember {

        /// <summary>
        /// Evaluates the member to an integer.
        /// </summary>
        int evalInt();

        /// <summary>
        /// Evaluates the member to a string.
        /// </summary>
        string evalString();
    }
}
