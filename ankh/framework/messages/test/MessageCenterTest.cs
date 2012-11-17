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
        [SetUp]
        public void ClearListeners()
        {
            MessageCenter<SomeTopic>.Clear();
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


        [Test]
        public void DisposedListenersShouldNotBeCalled()
        {
            int calls = 0;
            var sideEffectsListener = new Listener<SomeTopic>((x) => calls++);

            MessageCenter<SomeTopic>.Publish(new SomeTopic());

            sideEffectsListener.Dispose();

            MessageCenter<SomeTopic>.Publish(new SomeTopic());

            Assert.AreEqual(1, calls);
        }

        

        class SomeTopic : ITopic
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
    }
}
