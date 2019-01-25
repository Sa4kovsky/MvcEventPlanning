using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Concrete
{
    public class EFRepository : IRepository
    {
        EFDbContext context = new EFDbContext();

        public IEnumerable<Activity> Activities => context.Activities.Include(context => context.Subscriptions);

        public IEnumerable<Participant> Participants => context.Participants.Include(context => context.Subscriptions);

        public IEnumerable<Subscription> Subscriptions => context.Subscriptions;

        void IRepository.SaveActivity(Activity activity)
        {
            if (activity.ActivityId == 0)
                context.Activities.Add(activity);
            else
            {
                Activity dbEntry = context.Activities.Find(activity.ActivityId);
                if (dbEntry != null)
                {
                    dbEntry.NameEvent = activity.NameEvent;
                    dbEntry.DataTime = activity.DataTime;
                    dbEntry.NumberSeats = activity.NumberSeats;
                    dbEntry.Description = activity.Description;
                }
            }
            context.SaveChanges();
        }

        public Activity DeleteActivity(int activityId)
        {
            Activity dbEntry = context.Activities.Find(activityId);
            if (dbEntry != null)
            {
                context.Activities.Remove(dbEntry);
                context.SaveChanges();
            }
            return dbEntry;
        }

        void IRepository.SaveParticipants(Participant participant)
        {
            context.Participants.Add(participant);
            context.SaveChanges();
        }

        void IRepository.SaveSubcription(Subscription subscription)
        {
            context.Subscriptions.Add(subscription);
            context.SaveChanges();
        }
    }
}