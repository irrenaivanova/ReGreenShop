/// <reference types="node" />
import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import path from "path";
import fs from "fs";

const key = fs.readFileSync("certs/localhost-key.pem");
const cert = fs.readFileSync("certs/localhost.pem");

export default defineConfig({
  plugins: [react()],
  resolve: {
    alias: {
      "@": path.resolve(__dirname, "./src"),
    },
  },
  server: {
    https: {
      key,
      cert,
    },
    port: 5173,
  },
});
