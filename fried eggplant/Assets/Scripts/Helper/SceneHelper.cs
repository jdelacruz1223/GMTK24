/*
	The following license supersedes all notices in the source code.
	Copyright (c) 2021 Kurt Dekker/PLBM Games All rights reserved.
	http://www.twitter.com/kurtdekker
	Redistribution and use in source and binary forms, with or without
	modification, are permitted provided that the following conditions are
	met:
	Redistributions of source code must retain the above copyright notice,
	this list of conditions and the following disclaimer.
	Redistributions in binary form must reproduce the above copyright
	notice, this list of conditions and the following disclaimer in the
	documentation and/or other materials provided with the distribution.
	Neither the name of the Kurt Dekker/PLBM Games nor the names of its
	contributors may be used to endorse or promote products derived from
	this software without specific prior written permission.
	THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS
	IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
	TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A
	PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT
	HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
	SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED
	TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR
	PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF
	LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
	NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
	SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
*/


using Assets.Scripts.Util;

public static class SceneHelper
{
    public static void LoadScene(string s, bool additive = false, bool setActive = false)
    {
        if (s == null)
        {
            s = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            s, additive ? UnityEngine.SceneManagement.LoadSceneMode.Additive : 0);

        if (setActive)
        {
            // to mark it active we have to wait a frame for it to load.
            // Get the CallAfterDelay code at https://gist.github.com/kurtdekker/0da9a9721c15bd3af1d2ced0a367e24e
            CallAfterDelay.Create(0, () => {
                UnityEngine.SceneManagement.SceneManager.SetActiveScene(
                    UnityEngine.SceneManagement.SceneManager.GetSceneByName(s));
            });
        }
    }

    public static void UnloadScene(string s)
    {
        UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(s);
    }
}
