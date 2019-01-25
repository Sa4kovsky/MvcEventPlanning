using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Abstract
{
    public interface IRepository
    {
        IEnumerable<Activity> Activities { get; }
        IEnumerable<Participant> Participants { get; }
        IEnumerable<Subscription> Subscriptions { get; }

        void SaveActivity(Activity activity);
        Activity DeleteActivity(int activityId);

        void SaveParticipants(Participant participant);
        void SaveSubcription(Subscription subscription);
    }
}
