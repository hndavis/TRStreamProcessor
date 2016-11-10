using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TrsStreamsHost
{
    public class Filter
    {
        static Dictionary<string, List<IPublishing>> _subscribersList = new Dictionary<string, List<IPublishing>>();

       static public Dictionary<string, List<IPublishing>> SubscribersList
       {
           get {
               lock (typeof(Filter))
               {
                   return _subscribersList;
               }
}

       }

       static public List<IPublishing> GetSubscribers(String topicName)
{
    lock (typeof(Filter))
    {
        if (SubscribersList.ContainsKey(topicName))
        {
            return SubscribersList[topicName];
        }
        else
            return null;
    }
}

static public void AddSubscriber(String topicName, IPublishing subscriberCallbackReference)
{
    lock (typeof(Filter))
    {
        if (SubscribersList.ContainsKey(topicName))
        {
            if (!SubscribersList[topicName].Contains(subscriberCallbackReference))
            {
                SubscribersList[topicName].Add(subscriberCallbackReference);
            }
        }
        else
        {
            List< IPublishing > newSubscribersList = new List< IPublishing > ();
            newSubscribersList.Add(subscriberCallbackReference);
            SubscribersList.Add(topicName, newSubscribersList);
        }
    }

}

static public void RemoveSubscriber(String topicName, IPublishing subscriberCallbackReference)
{
    lock (typeof(Filter))
    {
        if (SubscribersList.ContainsKey(topicName))
        {
            if (SubscribersList[topicName].Contains(subscriberCallbackReference))
            {
                SubscribersList[topicName].Remove(subscriberCallbackReference);
            }
        }
    }
}

   }
}