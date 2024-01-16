﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Game.Utils.MessageBus
{
	public class MessageBus : MonoSingleton<MessageBus> {
		private Dictionary<MessageBusType, List<Action<Message>>> listeners = new Dictionary<MessageBusType, List<Action<Message>>>(50);
		private Queue<Message> normalMSGs = new Queue<Message>();
		private float lastBreakTime = 0;
		private float MAX_FRAME_TIME = 1f/60f;
		public static bool isInitialize = false;
		public void Initialize () {
			normalMSGs = new Queue<Message>(25);
			//timer.RunCoroutineOnInstance(ProcessMessage(), Segment.Update);
			isInitialize = true;
			isInit = true;
			//AdHelper.Instance.InitMessegeVip ();//TODO will be deleted
		}

		//IEnumerator<float> ProcessMessage () {
		//    while (true) {
		//        yield return 0f;
		//        lastBreakTime = Time.realtimeSinceStartup;
		//        while (normalMSGs.Count > 0) {
		//            float deltaBreakTime = Time.realtimeSinceStartup - lastBreakTime;
		//            if(deltaBreakTime >= MAX_FRAME_TIME) {
		//                Debug.LogWarning("<b><color=#ffcc11>Break frame time: </color></b>" + deltaBreakTime);
		//                break;
		//            }
		//            DispatchMessage(normalMSGs.Dequeue());
		//        }
		//    }
		//}

		private bool isInit;
		private void Update() {
			if(this.isInit == false) {
				return;
			}

			if(normalMSGs != null && this.normalMSGs.Count == 0) {
				return;
			}

			this.lastBreakTime = Time.realtimeSinceStartup;
			while(normalMSGs != null && this.normalMSGs.Count > 0) {
				float deltaBreakTime = Time.realtimeSinceStartup - lastBreakTime;
				if(deltaBreakTime >= this.MAX_FRAME_TIME) {
					//Debug.LogWarning("<b><color=#ffcc11>MessageBus - Break frame time: </color></b>" + deltaBreakTime);
					break;
				}

				DispatchMessage(normalMSGs.Dequeue());
			}
		}

		public void Subscribe(MessageBusType type, Action<Message> handler) {
			if (listeners.ContainsKey(type)) {
				if(listeners[type] == null) {
					listeners[type] = new List<Action<Message>>(20);
				}
			}else {
				listeners.Add(type, new List<Action<Message>>(20));
			}

			//print("Added handler for message " + type);
			listeners[type].Add(handler);
		}

		public void Unsubscribe(MessageBusType type, Action<Message> handler) {
			if (listeners.ContainsKey(type)) {
				if (listeners[type] != null) {
					listeners[type].Remove(handler);
				}
			}
		}

		/// <summary>
		/// Send a message to all subscribers listening to this certain message type
		/// </summary>
		/// <param name="message"></param>
		public void SendMessage(Message message, bool immidiately = false) {
			if (immidiately) {
				DispatchMessage(message);
			}else {
				normalMSGs.Enqueue(message);
			}
			////print("Sending message " + message.messageType);
			//if (listeners.ContainsKey(message.messageType)) {
			//    var listHandlers = listeners[message.messageType];
			//    //print("--List handler contains " + listHandlers.Count);
			//    for (int i = 0; i < listHandlers.Count; i++) {
			//        if(listHandlers[i] != null) {
			//            //print("--Message sent");
			//            listHandlers[i](message);
			//        }
			//    }
			//}
		}

		private void DispatchMessage(Message message) {
			List<Action<Message>> listHandlers = null;
			if(listeners.TryGetValue(message.messageType,out listHandlers)){
                List<Action<Message>> cloneList = new List<Action<Message>>(listHandlers);

                for (int i = cloneList.Count - 1; i>= 0; i--)
                {
                    if (cloneList[i] != null)
                    {
                        cloneList[i](message);
                    }
                }

				//for (int i = 0; i < listHandlers.Count; i++) {
				//	//print("--Message sent");
				//	if (listHandlers[i] != null) {
				//		listHandlers[i](message);
				//	}
				//}
			}
			/*if (listeners.ContainsKey(message.messageType)) {
                var listHandlers = listeners[message.messageType];
                //print("--List handler contains " + listHandlers.Count);
                for (int i = 0; i < listHandlers.Count; i++) {
                    //print("--Message sent");
                    if (listHandlers[i] != null) {
                        listHandlers[i](message);
                    }
                }
            }*/
		}


		/// <summary>
		/// Push a message directly into processing. Note that this call is synchronous and is not in framerate control
		/// </summary>
		/// <param name="message"></param>
		public static void AnnouceHighPriorityMessage (Message message) {
			Instance.DispatchMessage(message);
		}

		/// <summary>
		/// Short hand for calling send message with normal priority (will be delay if the target framerate is not reached)
		/// </summary>
		public static void Annouce(Message message) {
			Instance.SendMessage(message);
		}
		//Dictionary<MessageBusType, List<MessageSubscriber>> subscriberLists =
		//new Dictionary<MessageBusType, List<MessageSubscriber>>();

		//public void AddSubscriber(MessageSubscriber subscriber) {
		//    MessageBusType[] messageTypes = subscriber.MessageTypes;
		//    for (int i = 0; i < messageTypes.Length; i++)
		//        AddSubscriberToMessage(messageTypes[i], subscriber);
		//}

		//void AddSubscriberToMessage(MessageBusType messageType,
		//                             MessageSubscriber subscriber) {
		//    if (!subscriberLists.ContainsKey(messageType))
		//        subscriberLists[messageType] =
		//            new List<MessageSubscriber>();

		//    subscriberLists[messageType].Add(subscriber);
		//}

		//public void SendMessage(Message message) {
		//    if (!subscriberLists.ContainsKey(message.messageType))
		//        return;

		//    List<MessageSubscriber> subscriberList =
		//        subscriberLists[message.messageType];

		//    for (int i = 0; i < subscriberList.Count; i++)
		//        SendMessageToSubscriber(message, subscriberList[i]);
		//}

		//void SendMessageToSubscriber(Message message,
		//                             MessageSubscriber subscriber) {
		//    //subscriber.Handler.HandleMessage(message);
		//}

		///* Singleton */
		//static MessageBus instance;

		//public static MessageBus Instance {
		//    get {
		//        if (instance == null)
		//            instance = new MessageBus();

		//        return instance;
		//    }
		//}

		//private MessageBus() { }
	}
}