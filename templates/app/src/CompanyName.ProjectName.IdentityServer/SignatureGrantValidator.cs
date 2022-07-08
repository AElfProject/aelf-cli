using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AElf;
using AElf.Types;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Volo.Abp.Account;
using Volo.Abp.Identity;

namespace CompanyName.ProjectName.IdentityServer
{
    public class SignatureGrantValidator : IExtensionGrantValidator
    {
        private readonly ITokenValidator _validator;
        private readonly IdentityUserManager _identityUserManager;
        private readonly IIdentityUserRepository _identityUserRepository;
        private readonly IAccountAppService _accountAppService;

        public SignatureGrantValidator(ITokenValidator validator, IdentityUserManager identityUserManager, IIdentityUserRepository identityUserRepository)
        {
            _validator = validator;
            _identityUserManager = identityUserManager;
            _identityUserRepository = identityUserRepository;
        }

        public string GrantType => "signature";

        public async Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var pubkey = ByteArrayHelper.HexStringToByteArray(context.Request.Raw.Get("pubkey"));
            var signature = ByteArrayHelper.HexStringToByteArray(context.Request.Raw.Get("signature"));
            var timestamp =  long.Parse(context.Request.Raw.Get("timestamp"));
            var address = Address.FromPublicKey(pubkey).ToBase58();

            var time = DateTime.UnixEpoch.AddMilliseconds(timestamp);
            if (time < DateTime.UtcNow.AddMinutes(-5) || time> DateTime.UtcNow.AddMinutes(5))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }

            var hash = Encoding.UTF8.GetBytes(address+"-"+timestamp).ComputeHash();
            if (!AElf.Cryptography.CryptoHelper.VerifySignature(signature, hash, pubkey))
            {
                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant);
                return;
            }
            
            var user = await _identityUserRepository.FindByNormalizedUserNameAsync(address.ToUpper());
            if (user == null)
            {
                user = new IdentityUser(Guid.NewGuid(), address, address + "@projectname.com")
                {
                    Name = address
                };
                await _identityUserManager.CreateAsync(user);
            }

            var claims = new List<Claim>() {new Claim("role", GrantType)}; 
            context.Result = new GrantValidationResult(user.Id.ToString(), GrantType, claims);
        }
    }
}