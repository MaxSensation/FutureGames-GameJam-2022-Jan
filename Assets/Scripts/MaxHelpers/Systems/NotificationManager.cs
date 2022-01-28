using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace MaxHelpers
{
    public class NotificationManager : Singleton<NotificationManager>
    {
        [SerializeField] private VisualTreeAsset notificationAsset;
        
        private UIDocument _uiDocument;
        private Task _task;
        private readonly Queue<NotificationInfo> _queue = new();
        private CancellationTokenSource _tokenSource = new();

        private void Start()
        {
            _uiDocument = GetComponent<UIDocument>();
            _uiDocument.visualTreeAsset = notificationAsset;
        }

        private void Update()
        {
            if (_queue.Count == 0 || _task is not {IsCompleted: true}) return;
            var info = _queue.Dequeue();
            _task = RunTask(info.Header, info.Text, info.TimeDuration, Helper.RefreshToken(ref _tokenSource));
        }

        public void Notify(string header, string text, int timeDuration)
        {
            if (_task == null || _task.IsCompleted) _task = RunTask(header, text, timeDuration, Helper.RefreshToken(ref _tokenSource));
            else _queue.Enqueue(new NotificationInfo(header, text, timeDuration));
        }

        private async Task RunTask(string header, string text, int timeDuration, CancellationToken token)
        {
            _uiDocument.rootVisualElement.Q<VisualElement>("NotificationPanel").style.display = DisplayStyle.Flex;
            _uiDocument.rootVisualElement.Q<Label>("NotificationHeader").text = header;
            _uiDocument.rootVisualElement.Q<Label>("NotificationText").text = text;
            await Task.Delay(timeDuration, token);
            _uiDocument.rootVisualElement.Q<VisualElement>("NotificationPanel").style.display = DisplayStyle.None;
        }

        private void OnDestroy() => _tokenSource.Cancel();
    }

    internal struct NotificationInfo
    {
        public readonly string Header;
        public readonly string Text;
        public readonly int TimeDuration;
        public NotificationInfo(string header, string text, int timeDuration)
        {
            Header = header;
            Text = text;
            TimeDuration = timeDuration;
        }
    }
}