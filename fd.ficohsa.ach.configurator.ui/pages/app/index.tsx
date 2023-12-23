import { NextPage } from "next";

const Page: NextPage = () => {
  return <p>home</p>;
};

export default Page;
const requiredStandaloneDependencies = [
  // some required deps that have not been included in standalone
  "@opentelemetry",
  "semver",
  "yallist",
  "lru-cache",
  "shimmer",
  "resolve",
  "path-parse",
  "function-bind",
  "has",
  "is-core-module",
  "ms",
  "debug",
  "has-flag",
  "supports-color",
  "module-details-from-path",
  "require-in-the-middle",
  "@protobufjs",
  "protobufjs",
  "@grpc",
  "lodash.camelcase",
  "long",
  "thriftrw",
  "xtend",
  "ansi-color",
  "hexer",
  "bufrw",
  "string-template",
  "error",
  "jaeger-client",
  "opentracing",
  "xorshift",
  "node-int64",
];

export const config = {
  unstable_includeFiles: requiredStandaloneDependencies.map(
    (dep) => `node_modules/${dep}/**/*.+(js|json|proto|thrift)`
  ),
};
