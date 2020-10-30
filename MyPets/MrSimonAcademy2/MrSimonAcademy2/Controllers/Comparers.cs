using MrSimonAcademy2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MrSimonAcademy2.Controllers
{
    public class NewsComparer : IComparer<News>
    {
        public int Compare(News x, News y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {

                    // Сравневаемые параметры
                    int retval = y.added.CompareTo(x.added);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        // Если дата одинаковая (с точностью до секунд)
                        return retval;
                    }
                }
            }
        }
    }

    public class FileComparer : IComparer<Assignment>
    {
        public int Compare(Assignment x, Assignment y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
            else
            {
                if (y == null)
                {
                    return 1;
                }
                else
                {

                    // Сравневаемые параметры
                    int retval = y.added.CompareTo(x.added);

                    if (retval != 0)
                    {
                        return retval;
                    }
                    else
                    {
                        // Если дата одинаковая (с точностью до секунд)
                        return retval;
                    }
                }
            }
        }
    }
    
    public class GroupComparer : IComparer<Group>
    {
        public int Compare(Group x, Group y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the
                    // lengths of the two strings.
                    //
                    int retval = x.GroupName.Length.CompareTo(y.GroupName.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.GroupName.CompareTo(y.GroupName);
                    }
                }
            }
        }
    }

    public class UserComparer : IComparer<User>
    {
        public int Compare(User x, User y)
        {
            if (x == null)
            {
                if (y == null)
                {
                    // If x is null and y is null, they're
                    // equal.
                    return 0;
                }
                else
                {
                    // If x is null and y is not null, y
                    // is greater.
                    return -1;
                }
            }
            else
            {
                // If x is not null...
                //
                if (y == null)
                // ...and y is null, x is greater.
                {
                    return 1;
                }
                else
                {
                    // ...and y is not null, compare the
                    // lengths of the two strings.
                    //
                    int retval = x.UserLName.Length.CompareTo(y.UserLName.Length);

                    if (retval != 0)
                    {
                        // If the strings are not of equal length,
                        // the longer string is greater.
                        //
                        return retval;
                    }
                    else
                    {
                        // If the strings are of equal length,
                        // sort them with ordinary string comparison.
                        //
                        return x.UserLName.Length.CompareTo(y.UserLName.Length);
                    }
                }
            }
        }
    }

}