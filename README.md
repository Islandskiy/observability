## How to start

1. Start docker-compose from the repository root:
   ```bash
   docker-compose -f docker-compose.yml up
   ```
   This command runs the required infrastructure (Grafana, Grafana Loki, Promtail)
2. Open Grafana web page at http://localhost:3000 and login (default credentials are admin/admin)
3. Navigate to Configuration -> "Data sources" and add Loki data source. Specify just the Loki URL (http://loki:3100)
4. Start the app and make a request
5. Log messages should become visible in "Explore" tab in Grafana