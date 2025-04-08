namespace Services
{
    public interface IUserServices
    {
        Task<Response<List<User>>> GetUsers();
        Task<Response<User>> Get(string Name);
        Task<Response<User>> Create(UserCreateDto user);
    }

    public class UserServices(DataContext context) : ResponseService, IUserServices
    {
        private readonly DataContext _context = context;

        public async Task<Response<List<User>>> GetUsers()
        {
            var dbUser = await _context.User.ToListAsync();

            if (dbUser.Count == 0)
            {
                return ErrorResponse<List<User>>(MessageConst.UsersNoResults);
            }
            else
            {
                return SuccessResponse(dbUser, MessageConst.UsersSuccesConsult);
            }
        }

        public async Task<Response<User>> Get(string Name)
        {
            var dbUser = await _context.User.FirstOrDefaultAsync(u => u.Name == Name);

            if (dbUser == null)
            {
                return ErrorResponse<User>(String.Format(MessageConst.UserNoResult, Name));
            }
            else
            {
                return SuccessResponse(dbUser, MessageConst.UserSuccesConsult);
            }
        }

        public async Task<Response<User>> Create(UserCreateDto user)
        {
            var existingUser = await _context.User.FirstOrDefaultAsync(u => u.Name == user.Name);

            if (existingUser != null)
            {
                return InfoResponse<User>(MessageConst.UserAlreadyExists);
            }

            User newUser = new User
            {
                Name = user.Name,
                Balance = user.Balance,
                CreatedAt = user.CreatedAt,
            };

            _context.User.Add(newUser);
            await _context.SaveChangesAsync();

            var createdUser = await _context.User.FirstOrDefaultAsync(u => u.Id == newUser.Id);

            return SuccessResponse(
                createdUser!,
                String.Format(MessageConst.UserSuccesCreate, createdUser?.Name ?? "")
            );
        }
    }
}
