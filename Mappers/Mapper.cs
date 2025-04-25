using dotnet_simple_bank.Dtos.Balance;
using dotnet_simple_bank.Dtos.Transfer;
using dotnet_simple_bank.Dtos.User;
using dotnet_simple_bank.Models;

namespace dotnet_simple_bank.Mappers
{
    public static class Mapper
    {
        public static User CreateUserDtoToUser(CreateUserDto createUserDto)
        {
            return new User
            {
                UserName = createUserDto.Email,
                Email = createUserDto.Email,
                FullName = createUserDto.FullName,
                CpfCnpj = createUserDto.CpfCnpj,
                PhoneNumber = createUserDto.Phone,
            };
        }

        public static GetUserDto UserToGetUserDto(User user)
        {
            return new GetUserDto
            {
                Id = user.Id,
                Balance = user.Balance,
                FullName = user.FullName,
                CpfCnpj = user.CpfCnpj,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        public static CreateTransferResponseDto TransferToCreateTransferResponseDto(Transfer transfer)
        {
            return new CreateTransferResponseDto
            {
                Id = transfer.Id,
                Amount = transfer.Amount,
                PayerID = transfer.PayerID,
                PayeeID = transfer.PayeeID
            };
        }

        public static GetTransferDto TransferToGetTransferDto(Transfer transfer)
        {
            return new GetTransferDto
            {
                Id = transfer.Id,
                Amount = transfer.Amount,
                PayerID = transfer.PayeeID,
                PayeeID = transfer.PayeeID,
                CreatedAt = transfer.CreatedAt
            };
        }

        public static GetBalanceDto UserToGetBalanceDto(User user)
        {
            return new GetBalanceDto
            {
                Balance = user.Balance,
                FullName = user.FullName,
                CpfCnpj = user.CpfCnpj,
                Email = user.Email,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
