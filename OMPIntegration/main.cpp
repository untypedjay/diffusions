#include <iostream>
#include <chrono>

using namespace std;

inline double f(double x) {
	return 4.0 / (1 + x * x);
}

double integrateSeq(int n, double lb, double ub) {
	double sum = 0.0;
	double w = (ub - lb) / n;

	for (int i = 0; i < n; i++) {
		sum += w * f(lb + w * (i + 0.5));
	}
	return sum;
}

double integratePar(int n, double lb, double ub) {
	double sum = 0.0;
	double w = (ub - lb) / n;

#pragma omp parallel for reduction(+:sum) if (n > 5000)
	for (int i = 0; i < n; i++) {
		sum += w * f(lb + w * (i + 0.5));
	}
	return sum;
}

int main() {
	int n;
	cout << "n > ";
	cin >> n;
	cout << endl;

	while (n < 1000000000) {
		auto start = chrono::high_resolution_clock::now();
		double resSeq = integrateSeq(n, 0.0, 1.0);
		auto end = chrono::high_resolution_clock::now();
		chrono::duration<double, std::milli> time = end - start;
		double timeSeq = time.count();

		start = chrono::high_resolution_clock::now();
		double resPar = integratePar(n, 0.0, 1.0);
		end = chrono::high_resolution_clock::now();
		time = end - start;
		double timePar = time.count();

		cout << "n:       " << n << endl;
		cout << "Result:  " << resSeq << " (seq) \t" << resPar << " (OMP)" << endl;
		cout << "Time:    " << timeSeq << "ms (seq) \t" << timePar << " ms (OMP)"  << endl;
		cout << "Speedup: " << timeSeq / timePar << endl;
		cout << endl;

		n = n * 2;
	}
}