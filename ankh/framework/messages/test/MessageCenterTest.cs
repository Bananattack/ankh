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
            MessageCenter.Clear();
        }

        [Test]
        public void TopicCanBePublished()
        {
            var listener = new ListenerThing();
            
            var topic = new SomeTopic();
            topic.data = 500;
            MessageCenter.Publish(topic);

            Assert.AreEqual(listener.data, topic.data);
        }


        [Test]
        public void DisposedListenersShouldNotBeCalled()
        {
            int calls = 0;
            var sideEffectsListener = new Listener<SomeTopic>((x) => calls++);

            MessageCenter.Publish(new SomeTopic());

            sideEffectsListener.Dispose();

            MessageCenter.Publish(new SomeTopic());

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void OnlyListenersForSameTopicFire()
        {
            int calls = 0;
            var someTopicListener = new Listener<SomeTopic>((x) => calls++);
            var otherTopicListener = new Listener<OtherTopic>((x) => calls++);

            MessageCenter.Publish(new SomeTopic());

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void MultipleListenersForSameTopicAllFire()
        {
            int calls = 0;
            var someTopicListener = new Listener<SomeTopic>((x) => calls++);
            var anotherOne = new Listener<SomeTopic>((x) => calls++);

            MessageCenter.Publish(new SomeTopic());
            Assert.AreEqual(2, calls);
        }

        [Test]
        public void NoListeners()
        {
            Assert.DoesNotThrow(() => MessageCenter.Publish(new SomeTopic()));
        }
        class SomeTopic : ITopic
        {
            public int data;
        }
        class OtherTopic : ITopic
        {
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
