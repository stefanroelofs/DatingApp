namespace API.Data;

public class LikesRepository : ILikesRepository
{
    private readonly DataContext _context;

    public LikesRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<UserLike> GetUserLike(int sourceUserId, int likedUserId)
    {
        return await _context.Likes.FindAsync(sourceUserId, likedUserId);
    }

    public async Task<PagedList<LikeDto>> GetUserLikes(LikesParams likesParams)
    {
        var users = _context.Users.OrderBy(x => x.UserName).AsQueryable();
        var likes = _context.Likes.AsQueryable();

        if (likesParams.Predicate == "liked")
        {
            likes = likes.Where(x => x.SourceUserId == likesParams.UserId);
            users = likes.Select(x => x.LikedUser);
        }

        else if (likesParams.Predicate == "likedBy")
        {
            likes = likes.Where(x => x.LikedUserId == likesParams.UserId);
            users = likes.Select(x => x.SourceUser);
        }
        else
        {
            return null;
        }

        var likedUsers = users.Select(user => new LikeDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Age = user.DateOfBirth.CalculateAge(),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
            City = user.City,
            Id = user.Id,
        });

        return await PagedList<LikeDto>.CreateAsync(likedUsers, likesParams.PageNumber, likesParams.PageSize);
    }

    public async Task<AppUser> GetUserWithLikes(int userId)
    {
        return await _context.Users
            .Include(x => x.LikedUsers)
            // .FindAsync(userId);  // Not allowed when using a Include statement
            .FirstOrDefaultAsync(x => x.Id == userId);
    }
}
