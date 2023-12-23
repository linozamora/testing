import NextAuth, { type NextAuthOptions } from "next-auth";

export const authOptions: NextAuthOptions = {
  providers: [
    {
      id: "saml-jackson",
      name: "BoxyHQ",
      type: "oauth",
      checks: ["pkce", "state"],
      issuer: 'https://app.onelogin.com/saml/metadata/17f9f26a-a2bb-4d04-ab08-569e78f5d8f4',
      authorization: {
        url: `${process.env.NEXTAUTH_URL}/api/auth/saml/authorize`,
        params: {
          scope: "",
          response_type: "code",
          provider: "saml",
        },
      },
      token: {
        url: `${process.env.NEXTAUTH_URL}/api/auth/saml/token`,
        params: { grant_type: "authorization_code" },
      },
      userinfo: `${process.env.NEXTAUTH_URL}/api/auth/saml/userinfo`,
      profile: (profile) => {
        return {
          id: profile.id || "",
          firstName: profile.firstName || "",
          lastName: profile.lastName || "",
          email: profile.email || "",
          name: `${profile.firstName || ""} ${profile.lastName || ""}`.trim(),
          email_verified: true,
          role: "Admin",
        };
      },
      options: {
        clientId: "dummy",
        clientSecret: "dummy",
      },
    },
  ],
  session: {
    strategy: "jwt",
  },
  pages: {
    signIn: "/login"
  }
};

export default NextAuth(authOptions);
