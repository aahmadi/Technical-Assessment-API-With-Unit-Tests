using Ali.Planning.API.Repositories.Interfaces;
using Entities;

namespace Ali.Planning.API.Repositories
{
    public class ProjectRepository: 
        BaseRepository<Project>,
        IProjectRepository
    {
        public ProjectRepository(PlanningDataContext ctx)
            :base(ctx)
        { }
    }
}
