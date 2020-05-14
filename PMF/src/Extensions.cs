using PMF.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;

namespace PMF
{
    public static class Extensions
    {
        /// <summary>
        /// Nice hack to reuse this bit of code very effectively, this method is just used in List<T> where T is Package, otherwise this method doesn't even show up
        /// </summary>
        /// <param name="list">This is not an actual parameter</param>
        /// <param name="id">The id of the package</param>
        /// <returns>True if removed, false if not found</returns>
        public static bool Remove(this List<Package> list, string id)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID == id)
                {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// Same as Remove, but it Retrieves
        /// </summary>
        /// <param name="list">This is not an actual parameter</param>
        /// <param name="id">The id of the package</param>
        /// <returns>The package that was retrieved</returns>
        public static Package GetPackage(this List<Package> list, string id)
        {
            if (id == null || id.Length == 0)
                throw new ArgumentNullException();

            Package package = list.Find((p) => p.ID == id);
            if (package == null)
                throw new NotFoundException();

            return package;
        }
    }
}
