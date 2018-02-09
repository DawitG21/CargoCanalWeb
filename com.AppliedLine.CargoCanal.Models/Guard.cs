using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.AppliedLine.CargoCanal.Models
{
    public static class Guard
    {

        public static ArrayList LoggedInUsers = new ArrayList();

        //this happens when they login
        //the username along with a unique GUID is added to LoggedInUsers list
        /// <summary>
        /// Adds the username to the list of LoggedInUsers and assigns a unique Guid token
        /// </summary>
        /// <param name="username">The user name of the person who just logged in</param>
        /// <returns>LoginInfo Object</returns>
        public static LoginInfo addUserName(string username)
        {
           LoginInfo uf = new LoginInfo();

            //remove old userinfo if already there but copy the GUID
           LoginInfo oldUserInfo = new LoginInfo();
            oldUserInfo = getUserNameInfo(username);
            if (oldUserInfo != null)
            {
                uf._guid = oldUserInfo._guid;
                LoggedInUsers.Remove(oldUserInfo);
            }
            else {
                uf._guid = getGuid();
            }
            uf._username = username;
            uf._tick = DateTime.Now.Ticks.ToString();
            LoggedInUsers.Add(uf);
            return uf;
        }

        public static string getGuid()
        {
            return System.Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets the LoginInfo object for a username
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static LoginInfo getUserNameInfo(string username)
        {
            for (int i = 0; i <= LoggedInUsers.Count - 1; i++)
            {
                LoginInfo uInfo = new LoginInfo();
                uInfo = (LoginInfo)LoggedInUsers[i];
                if (uInfo._username == username)
                {
                    return uInfo;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks the list of LoginInfo objects to see if the passed Guid string exists
        /// </summary>
        /// <param name="_guid">The Guid string to check</param>
        /// <returns>True if Guid string is found, otherwise false</returns>
        public static bool canPass(string _guid)
        {
            for (int i = 0; i <= LoggedInUsers.Count - 1; i++)
            {
                LoginInfo uInfo = new LoginInfo();
                uInfo = (LoginInfo)LoggedInUsers[i];
                if (uInfo._guid == _guid)
                {
                    uInfo._tick = DateTime.Now.Ticks.ToString();
                    uInfo.lastSeenDateTime = DateTime.Now;
                    return true;
                }
            }
            return false;
        }

    }
}
