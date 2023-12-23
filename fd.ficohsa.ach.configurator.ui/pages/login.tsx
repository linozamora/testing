import { signIn } from "next-auth/react";
import { useRouter } from "next/router";
import { type ReactElement, useEffect } from "react";
import type { NextPageWithLayout } from "./_app";

const Login: NextPageWithLayout = () => {
  const router = useRouter();

  useEffect(() => {
    if (!router.isReady) return;

    signIn(
      "saml-jackson",
      { callbackUrl: "/app", code: router.query?.code },
      {
        tenant: "ach",
        product: "ach-configurator",
      }
    );
  }, [router.isReady, router.query]);

  return null;
};

Login.getLayout = function getLayout(page: ReactElement) {
  return page;
};
export default Login;
