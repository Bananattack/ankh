using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ankh.framework.messages.test
{
    [TestFixture]
    class MessageCenterTest
    {
        class SomeTopic : Topic
        {
            public int data;
        }

        class ListenerThing
        {
            public ListenerThing()
            {
                someTopicListener = new Listener<SomeTopic>(topic => this.data = topic.data);
            }

            public int data = 0;
            Listener<SomeTopic> someTopicListener;
        }

        [Test]
        public void TopicCanBePublished()
        {
            var listener = new ListenerThing();
            
            var topic = new SomeTopic();
            topic.data = 500;
            MessageCenter<SomeTopic>.Publish(topic);

            Assert.AreEqual(listener.data, topic.data);
        }
    }
}
